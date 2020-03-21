using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using FileCabinetApp.RecordPrinter;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer) 
            : base(service)
        {
            this.printer = printer;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("list", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = this.service.GetRecords();
            this.printer.Print(arrayOfRecords);
        }
    }
}
