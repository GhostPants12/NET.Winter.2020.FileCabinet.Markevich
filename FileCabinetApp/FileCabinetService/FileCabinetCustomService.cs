using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp
{
    /// <summary>Custom implementation for the FileCabinetService.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetMemoryService" />
    public class FileCabinetCustomService : FileCabinetMemoryService
    {
        /// <summary>Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.</summary>
        public FileCabinetCustomService()
            : base(new ValidatorBuilder().CreateCustom())
        {
        }
    }
}
