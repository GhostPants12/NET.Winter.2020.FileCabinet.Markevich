using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.IRecordValidator;

namespace FileCabinetApp
{
    /// <summary>Default implementation for the FileCabinetService.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.</summary>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
