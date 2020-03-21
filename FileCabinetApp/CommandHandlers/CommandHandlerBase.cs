using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        internal ICommandHandler nextHandler;

        public abstract void Handle(AppCommandRequest request);

        public void SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
        }
    }
}
