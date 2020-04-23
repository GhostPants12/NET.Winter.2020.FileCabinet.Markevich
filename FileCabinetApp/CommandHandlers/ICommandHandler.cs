using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Interface for command handlers.</summary>
    public interface ICommandHandler
    {
        /// <summary>Sets the next handler.</summary>
        /// <param name="handler">The handler.</param>
        void SetNext(ICommandHandler handler);

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        void Handle(AppCommandRequest request);
    }
}
