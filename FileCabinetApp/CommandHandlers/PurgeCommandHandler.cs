using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {

        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("purge", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            try
            {
                int purged = this.service.Purge();
                Console.WriteLine($"{purged} elements from {this.service.GetStat() + purged} were purged.");
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
