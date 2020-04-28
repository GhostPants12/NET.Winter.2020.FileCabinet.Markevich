using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Base class for the command handlers.</summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        /// <summary>The next handler.</summary>
        #pragma warning disable SA1401 // Fields should be private
        internal ICommandHandler NextHandler;
        #pragma warning restore SA1401 // Fields should be private

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public abstract void Handle(AppCommandRequest request);

        /// <summary>Sets the next handler.</summary>
        /// <param name="handler">The handler.</param>
        public void SetNext(ICommandHandler handler)
        {
            this.NextHandler = handler;
        }
    }
}
