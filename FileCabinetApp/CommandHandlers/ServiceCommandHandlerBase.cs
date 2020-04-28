using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Base class for command handlers that interact with IFileCabinetService classes.</summary>
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>The service.</summary>
#pragma warning disable CA1051 // Do not declare visible instance fields
#pragma warning disable SA1401 // Fields should be private
        protected IFileCabinetService service;
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore CA1051 // Do not declare visible instance fields

        /// <summary>Initializes a new instance of the <see cref="ServiceCommandHandlerBase" /> class.</summary>
        /// <param name="service">The service.</param>
        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public override void Handle(AppCommandRequest request)
        {
        }
    }
}
