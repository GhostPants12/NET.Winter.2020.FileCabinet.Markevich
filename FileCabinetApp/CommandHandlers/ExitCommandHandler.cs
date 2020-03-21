using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> close;

        public ExitCommandHandler(Action<bool> close)
        {
            this.close = close;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            Console.WriteLine("Exiting an application...");
            close?.Invoke(false);
        }
    }
}
