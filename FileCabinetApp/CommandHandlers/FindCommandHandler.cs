using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        public FindCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
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
                IEnumerable<FileCabinetRecord> arrayOfRecords = this.service.FindByFirstName(valueToFind);
                this.printer?.Invoke(arrayOfRecords);
            }

            if (propertyName.Equals("lastname ", StringComparison.InvariantCultureIgnoreCase))
            {
                IEnumerable<FileCabinetRecord> arrayOfRecords = this.service.FindByLastName(valueToFind);
                this.printer?.Invoke(arrayOfRecords);
            }

            if (propertyName.Equals("dateofbirth ", StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime parameterDateTime = DateTime.ParseExact(valueToFind, "yyyy-MMM-dd", CultureInfo.InvariantCulture);
                IEnumerable<FileCabinetRecord> arrayOfRecords = this.service.FindByDateOfBirth(parameterDateTime);
                this.printer?.Invoke(arrayOfRecords);
            }
        }
    }
}
