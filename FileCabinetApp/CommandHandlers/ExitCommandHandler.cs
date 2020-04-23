using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for exit command.</summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        /// <summary>Delegate for closing the program.</summary>
        private readonly Action<bool> close;

        /// <summary>Initializes a new instance of the <see cref="ExitCommandHandler" /> class.</summary>
        /// <param name="close">The close.</param>
        public ExitCommandHandler(Action<bool> close)
        {
            this.close = close;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            #pragma warning disable CA1303 // Do not pass literals as localized parameters
            Console.WriteLine("Exiting an application...");
            #pragma warning restore CA1303 // Do not pass literals as localized parameters
            this.close?.Invoke(false);
        }
    }
}
