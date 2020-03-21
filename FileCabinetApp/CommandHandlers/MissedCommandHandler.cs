using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class MissedCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            Console.WriteLine($"There is no '{request.Command}' command.");
            Console.WriteLine();
        }
    }
}
