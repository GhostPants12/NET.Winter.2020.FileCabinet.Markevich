using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public FindCommandHandler(IFileCabinetService serivce)
        {
            this.service = serivce;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("find", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            string propertyName = string.Empty;
            string valueToFind = string.Empty;
            try
            {
                string[] parametersArray = request.Parameters.Split('"');
                propertyName = parametersArray[0];
                valueToFind = parametersArray[1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command parameters are incorrect.");
            }

            if (propertyName.Equals("firstname ", StringComparison.InvariantCultureIgnoreCase))
            {
                ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = this.service.FindByFirstName(valueToFind);
                foreach (FileCabinetRecord record in arrayOfRecords)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
                }
            }

            if (propertyName.Equals("lastname ", StringComparison.InvariantCultureIgnoreCase))
            {
                ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = this.service.FindByLastName(valueToFind);
                foreach (FileCabinetRecord record in arrayOfRecords)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
                }
            }

            if (propertyName.Equals("dateofbirth ", StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime parameterDateTime = DateTime.ParseExact(valueToFind, "yyyy-MMM-dd", CultureInfo.InvariantCulture);
                ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = this.service.FindByDateOfBirth(parameterDateTime);
                foreach (FileCabinetRecord record in arrayOfRecords)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
                }
            }
        }
    }
}
