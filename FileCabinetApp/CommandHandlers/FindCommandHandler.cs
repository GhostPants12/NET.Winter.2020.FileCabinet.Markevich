using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using FileCabinetApp.RecordPrinter;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
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
                this.printer.Print(arrayOfRecords);
            }

            if (propertyName.Equals("lastname ", StringComparison.InvariantCultureIgnoreCase))
            {
                ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = this.service.FindByLastName(valueToFind);
                this.printer.Print(arrayOfRecords);
            }

            if (propertyName.Equals("dateofbirth ", StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime parameterDateTime = DateTime.ParseExact(valueToFind, "yyyy-MMM-dd", CultureInfo.InvariantCulture);
                ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = this.service.FindByDateOfBirth(parameterDateTime);
                this.printer.Print(arrayOfRecords);
            }
        }
    }
}
