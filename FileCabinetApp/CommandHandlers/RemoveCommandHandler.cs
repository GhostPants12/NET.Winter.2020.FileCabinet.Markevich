using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        public RemoveCommandHandler(IFileCabinetService service)
            : base(service)
        {

        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("remove", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            try
            {
                int id = int.Parse(request.Parameters, CultureInfo.InvariantCulture);
                this.service.DeleteRecord(id);
                Console.WriteLine($"Record #{id} was successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
