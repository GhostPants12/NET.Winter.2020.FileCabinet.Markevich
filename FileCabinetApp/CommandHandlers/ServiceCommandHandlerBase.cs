using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected IFileCabinetService service;

        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest request)
        {

        }
    }
}
