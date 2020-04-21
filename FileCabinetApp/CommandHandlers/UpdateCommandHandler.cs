using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers
{
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        public UpdateCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("update", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            List<IEnumerable<FileCabinetRecord>> collectionsList = new List<IEnumerable<FileCabinetRecord>>();
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
                                var elementCollection = new List<FileCabinetRecord> {element};
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

            var andCollection = new List<IEnumerable<FileCabinetRecord>>();
            var streak = false;
            for (int i = 0; i < boolOperatorsList.Count; i++)
            {
                if (boolOperatorsList[i].Equals("and", StringComparison.InvariantCultureIgnoreCase))
                {
                    collectionsList[i] = collectionsList[i].Intersect(collectionsList[i + 1]);
                    collectionsList[i + 1] = collectionsList[i];
                    if (!streak)
                    {
                        andCollection.Add(collectionsList[i]);
                        streak = true;
                        continue;
                    }

                    andCollection[andCollection.Count - 1] = andCollection[andCollection.Count - 1].Intersect(collectionsList[i]);
                    continue;
                }

                streak = false;
            }

            if (andCollection.Count == 0)
            {
                andCollection = collectionsList;
            }

            foreach (var element in collectionsList.Distinct().Except(andCollection))
            {
                andCollection.Add(element);
            }

            var resultList = andCollection[0];
            var index = 0;

            foreach (var boolOperation in boolOperatorsList)
            {
                if (boolOperation.Equals("or", StringComparison.InvariantCultureIgnoreCase))
                {
                    resultList = resultList.Union(andCollection[index].Union(andCollection[++index])).ToList();
                }
            }

            foreach (var element in resultList)
            {
                foreach (var param in Regex.Split(parameters, @",(\s)*"))
                {
                    if (param.Equals(" ", StringComparison.InvariantCultureIgnoreCase)) continue;
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
        }
    }
}
