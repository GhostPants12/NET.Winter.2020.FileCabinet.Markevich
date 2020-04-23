using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for the insert command.</summary>
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="InsertCommandHandler" /> class.</summary>
        /// <param name="service">The service.</param>
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentException">Some of the parameters are missing in the {request.Parameters}.</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("insert", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            bool[] isFull = new bool[6];
            string firstName = string.Empty;
            string lastName = string.Empty;
            short code = -1;
            char letter = ' ';
            decimal balance = -1;
            DateTime dateOfBirth = default;
            if (request != null)
            {
                string[] commandArray = Regex.Split(request.Parameters, "( values )");
                for (int i = 0; i < commandArray.Length; i++)
                {
                    commandArray[i] = Regex.Replace(commandArray[i], "[()]", string.Empty);
                }

                string[] paramsArray = commandArray[0].Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase).Split(",");
                string[] valueArray = commandArray[2].Split(',');
                for (int i = 0; i < valueArray.Length; i++)
                {
                    valueArray[i] = Regex.Replace(valueArray[i], @"(\s+)'", "'").Replace("'", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                }

                for (int i = 0; i < paramsArray.Length; i++)
                {
                    if (paramsArray[i].Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
                    {
                        firstName = valueArray[i];
                        isFull[0] = true;
                    }

                    if (paramsArray[i].Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lastName = valueArray[i];
                        isFull[1] = true;
                    }

                    if (paramsArray[i].Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!DateTime.TryParseExact(valueArray[i], "M/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                        {
                            throw new ArgumentException($"{valueArray[i]} is an incorrect date of birth.");
                        }

                        dateOfBirth = time;
                        isFull[2] = true;
                    }

                    if (paramsArray[i].Equals("code", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!short.TryParse(valueArray[i], out code))
                        {
                            throw new ArgumentException($"{valueArray[i]} is an incorrect code.");
                        }

                        isFull[3] = true;
                    }

                    if (paramsArray[i].Equals("letter", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!char.TryParse(valueArray[i], out letter))
                        {
                            throw new ArgumentException($"{valueArray[i]} is an incorrect letter.");
                        }

                        isFull[4] = true;
                    }

                    if (paramsArray[i].Equals("balance", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!decimal.TryParse(valueArray[i], out balance))
                        {
                            throw new ArgumentException($"{valueArray[i]} is an incorrect balance.");
                        }

                        isFull[5] = true;
                    }
                }
            }

            if (isFull.All(b => !b))
            {
                if (request != null)
                {
                    throw new ArgumentException($"Some of the parameters are missing in the {request.Parameters}.");
                }
            }

            RecordData recordDataToCreate = new RecordData(firstName, lastName, code, letter, balance, dateOfBirth);
            int id = this.service.CreateRecord(recordDataToCreate);
            Console.WriteLine($"Record #{id} has been created.");
        }
    }
}
