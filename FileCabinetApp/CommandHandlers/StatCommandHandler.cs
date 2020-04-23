using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for stat command.</summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="StatCommandHandler" /> class.</summary>
        /// <param name="service">The service.</param>
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("stat", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            var recordsCount = this.service.GetStat();
            var recordsToRemoveCount = this.service.GetRemovedStat();
            Console.WriteLine($"{recordsCount} record(s).");
            if (recordsToRemoveCount == 0)
            {
                #pragma warning disable CA1303 // Do not pass literals as localized parameters
                Console.WriteLine("There are no records to remove.");
                #pragma warning restore CA1303 // Do not pass literals as localized parameters
                return;
            }

            Console.WriteLine($"There are {recordsToRemoveCount} records to remove.");
        }
    }
}
