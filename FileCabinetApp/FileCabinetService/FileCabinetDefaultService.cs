using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp
{
    /// <summary>Default implementation for the FileCabinetService.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetMemoryService" />
    public class FileCabinetDefaultService : FileCabinetMemoryService
    {
        /// <summary>Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.</summary>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
