using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers
{
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        public DeleteCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

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
