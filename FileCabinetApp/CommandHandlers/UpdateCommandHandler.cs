using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for update command.</summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="UpdateCommandHandler" /> class.</summary>
        /// <param name="service">The service.</param>
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentException">Thrown when parameters are incorrect.</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("update", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            List<IEnumerable<FileCabinetRecord>> collectionsList = new List<IEnumerable<FileCabinetRecord>>();
            if (request != null)
            {
                var parameters = Regex.Split(request.Parameters, @"set(\s)*")[^1];
                List<string> boolOperatorsList = new List<string>();
                List<string> conditionsList = new List<string>();
                var conditions = Regex.Split(parameters, @"((\s)*where(\s)*)")[^1];
                parameters = Regex.Split(parameters, @"(\s)*where(\s*)")[0];
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
                        collectionsList.Add(this.service.FindByFirstName(conditionValue));
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

                if (boolOperatorsList.Count == 0)
                {
                    orCollection.Add(collectionsList[0]);
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

                foreach (var element in resultList)
                {
                    foreach (var param in Regex.Split(parameters, @",(\s)*"))
                    {
                        if (param.Equals(" ", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        var conditionArguments = param.Split('=', 2);
                        var conditionKey = conditionArguments[0]
                            .Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                        var conditionValue = conditionArguments[1].Split(@"'")[1];
                        if (conditionKey.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
                        {
                            element.FirstName = conditionValue;
                            this.service.EditRecord(new RecordData(element.FirstName, element.LastName, element.Code, element.Letter, element.Balance, element.DateOfBirth) { Id = element.Id });
                        }

                        if (conditionKey.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
                        {
                            element.LastName = conditionValue;
                            this.service.EditRecord(new RecordData(element.FirstName, element.LastName, element.Code, element.Letter, element.Balance, element.DateOfBirth) { Id = element.Id });
                        }

                        if (conditionKey.Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!DateTime.TryParseExact(conditionValue, "M/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                            {
                                throw new ArgumentException($"{conditionValue} is an incorrect date of birth.");
                            }

                            element.DateOfBirth = time;
                            this.service.EditRecord(new RecordData(element.FirstName, element.LastName, element.Code, element.Letter, element.Balance, element.DateOfBirth) { Id = element.Id });
                        }

                        if (conditionKey.Equals("code", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!short.TryParse(conditionValue, out var code))
                            {
                                throw new ArgumentException($"{conditionValue} is an incorrect code.");
                            }

                            element.Code = code;
                            this.service.EditRecord(new RecordData(element.FirstName, element.LastName, element.Code, element.Letter, element.Balance, element.DateOfBirth) { Id = element.Id });
                        }

                        if (conditionKey.Equals("letter", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!char.TryParse(conditionValue, out var letter))
                            {
                                throw new ArgumentException($"{conditionValue} is an incorrect letter.");
                            }

                            element.Letter = letter;
                            this.service.EditRecord(new RecordData(element.FirstName, element.LastName, element.Code, element.Letter, element.Balance, element.DateOfBirth) { Id = element.Id });
                        }

                        if (conditionKey.Equals("balance", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!decimal.TryParse(conditionValue, out var balance))
                            {
                                throw new ArgumentException($"{conditionValue} is an incorrect balance.");
                            }

                            element.Balance = balance;
                            this.service.EditRecord(new RecordData(element.FirstName, element.LastName, element.Code, element.Letter, element.Balance, element.DateOfBirth) { Id = element.Id });
                        }
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
}
