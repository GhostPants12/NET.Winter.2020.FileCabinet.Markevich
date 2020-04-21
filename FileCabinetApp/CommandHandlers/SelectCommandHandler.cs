using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp.CommandHandlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        public SelectCommandHandler(IFileCabinetService service) 
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("select", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            Dictionary<int, string> numberParamsDictionary = new Dictionary<int, string>()
            {
                { 0, "Id"},
                { 1, "FirstName" },
                { 2, "LastName" },
                { 3, "DateOfBirth" },
                { 4, "Code" },
                { 5, "Letter" },
                { 6, "Balance" },
            };
            List<IEnumerable<FileCabinetRecord>> collectionsList = new List<IEnumerable<FileCabinetRecord>>();
            List<string> boolOperatorsList = new List<string>();
            List<string> conditionsList = new List<string>();
            var conditions = Regex.Split(request.Parameters, @"((\s)*where(\s)*)")[^1];
            var parameters = Regex.Split(request.Parameters, @"(\s)*where(\s*)")[0];
            foreach (Match match in Regex.Matches(conditions, @"(\w)+(\s)*=(\s)*'[(\w)|(\s)]+'(\s)*(\w)+(\s)*"))
            {
                conditionsList.Add(Regex.Match(match.Value, @"^((\w)+(\s)*=(\s)*'[(\w)|(\s)]+')").Value);
                boolOperatorsList.Add(Regex.Match(match.Value, @"(\w)+(\s)*$").Value);
            }

            conditionsList.Add(Regex.Match(conditions, @"((\w)+(\s)*=(\s)*'[(\w)|(\s)]+')$").Value);
            for (int i = 0; i < conditionsList.Count; i++)
            {
                var conditionArguments = conditionsList[i].Split('=', 2);
                var conditionKey = conditionArguments[0]
                    .Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                var conditionValue = conditionArguments[1].Split(@"'")[1];
                if (conditionKey.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
                {
                    collectionsList.Add(this.service.FindByFirstName(conditionValue));
                    continue;
                }

                if (conditionKey.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
                {
                    collectionsList.Add(this.service.FindByLastName(conditionValue));
                    continue;
                }

                if (conditionKey.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (int.TryParse(conditionValue, out var id))
                    {
                        foreach (var element in this.service.GetRecords())
                        {
                            if (element.Id == id)
                            {
                                var elementCollection = new List<FileCabinetRecord> { element };
                                collectionsList.Add(elementCollection);
                                continue;
                            }
                        }
                    }
                }

                if (conditionKey.Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!DateTime.TryParseExact(conditionValue, "M/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                    {
                        throw new ArgumentException($"{conditionValue} is an incorrect date of birth.");
                    }

                    collectionsList.Add(this.service.FindByDateOfBirth(time));
                }
            }

            var orCollection = new List<IEnumerable<FileCabinetRecord>>();
            List<FileCabinetRecord> andEnumerable = new List<FileCabinetRecord>();
            bool streak = false;
            for (int i = 0; i < boolOperatorsList.Count; i++)
            {
                if (boolOperatorsList[i].Replace(" ", string.Empty, StringComparison.InvariantCulture).Equals("or", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (streak)
                    {
                        orCollection.Add(andEnumerable);
                    }

                    if (!streak)
                    {
                        orCollection.Add(collectionsList[i]);
                        streak = false;
                    }

                    if (i + 1 == boolOperatorsList.Count)
                    {
                        orCollection.Add(collectionsList[i + 1]);
                    }
                }

                if (boolOperatorsList[i].Replace(" ", string.Empty, StringComparison.InvariantCulture)
                    .Equals("and", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!streak)
                    {
                        andEnumerable =
                            new List<FileCabinetRecord>(Intersect(collectionsList[i], collectionsList[i + 1]));
                        streak = true;
                    }

                    if (streak)
                    {
                        andEnumerable = Intersect(andEnumerable, collectionsList[i + 1]).ToList();
                    }

                    if (i + 1 == boolOperatorsList.Count)
                    {
                        orCollection.Add(andEnumerable);
                    }
                }
            }

            List<FileCabinetRecord> resultList = orCollection[0].ToList();
            var index = 0;

            foreach (var boolOperation in boolOperatorsList)
            {
                if (boolOperation.Replace(" ", string.Empty, StringComparison.InvariantCulture)
                    .Equals("or", StringComparison.InvariantCultureIgnoreCase))
                {
                    resultList = resultList.Union(orCollection[index].Union(orCollection[++index])).ToList();
                }
            }

            List<int> toRemove = new List<int>();
            for (int i = 0; i < resultList.Count; i++)
            {
                for (int j = i + 1; j < resultList.Count; j++)
                {
                    if (resultList[i].Id == resultList[j].Id)
                    {
                        toRemove.Add(j);
                    }
                }
            }

            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            for (int i = 0; i < resultList.Count; i++)
            {
                if (!toRemove.Contains(i))
                {
                    result.Add(resultList[i]);
                }
            }

            resultList = result;

            var rows = new bool[7];

            foreach (var param in Regex.Split(parameters, @",(\s)*"))
            {
                if (param.Equals(" ", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                for (int i = 0; i <= numberParamsDictionary.Keys.Max(); i++)
                {
                    if (param.Equals(numberParamsDictionary[i], StringComparison.InvariantCultureIgnoreCase))
                    {
                        rows[i] = true;
                    }
                }
            }

            int[] maxLengthArray = new int[7];
            StringBuilder dividerStringBuilder = new StringBuilder();
            dividerStringBuilder.Append(" +");
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i])
                {
                    int length = 0;
                    switch (i)
                    {
                        case 0:
                            length = resultList.Max(cabinetRecord =>
                                cabinetRecord.Id.ToString(CultureInfo.InvariantCulture).Length);
                            break;
                        case 1:
                            length = resultList.Max(cabinetRecord => cabinetRecord.FirstName.Length);
                            break;
                        case 2:
                            length = resultList.Max(cabinetRecord => cabinetRecord.LastName.Length);
                            break;
                        case 3:
                            length = resultList.Max(cabinetRecord =>
                                cabinetRecord.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length);
                            break;
                        case 4:
                            length = resultList.Max(cabinetRecord =>
                                cabinetRecord.Code.ToString(CultureInfo.InvariantCulture).Length);
                            break;
                        case 5:
                            length = 1;
                            break;
                        case 6:
                            length = resultList.Max(cabinetRecord =>
                                cabinetRecord.Balance.ToString(CultureInfo.InvariantCulture).Length);
                            break;
                    }

                    length = length > numberParamsDictionary[i].Length
                        ? length + 2
                        : numberParamsDictionary[i].Length + 2;
                    for (int j = 0;
                        j < length;
                        j++)
                    {
                        dividerStringBuilder.Append("-");
                    }

                    maxLengthArray[i] = length - 2;
                    dividerStringBuilder.Append("+");
                }
            }

            Tuple<string, int[]> divider = new Tuple<string, int[]>(dividerStringBuilder.ToString(), maxLengthArray);

            Console.WriteLine(divider.Item1);

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i])
                {
                    Console.Write(" | " + numberParamsDictionary[i]);
                    for (int j = 0; j < divider.Item2[i] - numberParamsDictionary[i].Length; j++)
                    {
                        Console.Write(" ");
                    }
                }
            }

            Console.Write(" |");
            Console.WriteLine();
            Console.WriteLine(divider.Item1);
            foreach (var record in resultList)
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    if (rows[i])
                    {
                        switch (i)
                        {
                            case 0:
                                WriteRow(record.Id.ToString(CultureInfo.InvariantCulture), i);
                                break;
                            case 1:
                                WriteRow(record.FirstName, i);
                                break;
                            case 2:
                                WriteRow(record.LastName, i);
                                break;
                            case 3:
                                WriteRow(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), i);
                                break;
                            case 4:
                                WriteRow(record.Code.ToString(CultureInfo.InvariantCulture), i);
                                break;
                            case 5:
                                WriteRow(record.Letter.ToString(CultureInfo.InvariantCulture), i);
                                break;
                            case 6:
                                WriteRow(record.Balance.ToString(CultureInfo.InvariantCulture), i);
                                break;
                        }
                    }
                }

                Console.Write(" |");
                Console.WriteLine();
                Console.WriteLine(divider.Item1);
            }

            void WriteRow(string element, int counter)
            {
                Console.Write(" | ");
                if (counter == 1 || counter == 2)
                {
                    Console.Write(element);
                    for (int j = 0; j < divider.Item2[counter] - element.Length; j++)
                    {
                        Console.Write(" ");
                    }
                }
                else
                {
                    for (int j = 0; j < divider.Item2[counter] - element.Length; j++)
                    {
                        Console.Write(" ");
                    }

                    Console.Write(element);
                }
            }

            IEnumerable<FileCabinetRecord> Intersect(IEnumerable<FileCabinetRecord> targetList, IEnumerable<FileCabinetRecord> sourceList)
            {
                List<FileCabinetRecord> result = new List<FileCabinetRecord>();
                foreach (var record in targetList)
                {
                    foreach (var sourceRecord in sourceList)
                    {
                        if (record.Id == sourceRecord.Id)
                        {
                            result.Add(record);
                        }
                    }
                }

                return result;
            }
        }
    }
}
