using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("purge", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            try
            {
                int purged = Program.fileCabinetService.Purge();
                Console.WriteLine($"{purged} elements from {Program.fileCabinetService.GetStat() + purged} were purged.");
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
