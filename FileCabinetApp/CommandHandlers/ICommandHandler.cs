using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public interface ICommandHandler
    {
        void SetNext(ICommandHandler handler);

        void Handle(AppCommandRequest request);
    }
}
