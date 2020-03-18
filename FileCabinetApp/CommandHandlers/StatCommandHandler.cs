using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("stat", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            var recordsCount = Program.fileCabinetService.GetStat();
            var recordsToRemoveCount = Program.fileCabinetService.GetRemovedStat();
            Console.WriteLine($"{recordsCount} record(s).");
            if (recordsToRemoveCount == 0)
            {
                Console.WriteLine("There are no records to remove.");
                return;
            }

            Console.WriteLine($"There are {recordsToRemoveCount} records to remove.");
        }
    }
}
