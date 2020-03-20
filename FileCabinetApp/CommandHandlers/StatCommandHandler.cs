using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public StatCommandHandler(IFileCabinetService serivce)
        {
            this.service = serivce;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("stat", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            var recordsCount = this.service.GetStat();
            var recordsToRemoveCount = this.service.GetRemovedStat();
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
