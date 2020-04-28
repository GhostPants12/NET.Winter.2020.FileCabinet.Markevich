using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for the delete command.</summary>
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="DeleteCommandHandler" /> class.</summary>
        /// <param name="service">The service.</param>
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentException">Thrown when parameters are incorrect.</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            if (request != null)
            {
                string[] parameters = request.Parameters.Split(' ', 2);
                if (!parameters[0].Equals("where", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ArgumentException($"{request.Parameters} is an incorrect command.");
                }

                Match keyValueString = Regex.Match(parameters[1], @"(\w+)(\s)*=(\s)*(\S+)");
                string[] keyValueStrings = Regex.Split(keyValueString.Value, @"(\s)*=(\s)*");
                var value = keyValueStrings[^1].Split(@"'")[1];
                if (keyValueStrings[0].Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (var element in this.service.FindByFirstName(value))
                    {
                        this.service.DeleteRecord(element.Id);
                        Console.WriteLine($"Record#{element.Id} was deleted.");
                    }
                }

                if (keyValueStrings[0].Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (var element in this.service.FindByLastName(value))
                    {
                        this.service.DeleteRecord(element.Id);
                        Console.WriteLine($"Record#{element.Id} was deleted.");
                    }
                }

                if (keyValueStrings[0].Equals("id", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!int.TryParse(value, out var id))
                    {
                        throw new ArgumentException($"{value} is an incorrect id.");
                    }

                    this.service.DeleteRecord(id);
                    Console.WriteLine($"Record#{id} was deleted.");
                }

                if (keyValueStrings[0].Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!DateTime.TryParseExact(value, "M/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                    {
                        throw new ArgumentException($"{value} is an incorrect date of birth.");
                    }

                    foreach (var element in this.service.FindByDateOfBirth(time))
                    {
                        this.service.DeleteRecord(element.Id);
                        Console.WriteLine($"Record#{element.Id} was deleted.");
                    }
                }
            }
        }
    }
}
