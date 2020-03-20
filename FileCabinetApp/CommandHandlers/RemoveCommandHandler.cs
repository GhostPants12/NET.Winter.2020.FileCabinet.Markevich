using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public RemoveCommandHandler(IFileCabinetService serivce)
        {
            this.service = serivce;
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
