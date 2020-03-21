using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer) 
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
            this.printer?.Invoke(arrayOfRecords);
        }
    }
}
