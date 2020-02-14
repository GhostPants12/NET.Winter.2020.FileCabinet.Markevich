using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.IRecordValidator;

namespace FileCabinetApp
{
    /// <summary>Custom implementation for the FileCabinetService.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.</summary>
        public FileCabinetCustomService()
            : base(new CustomValidator())
        {
        }
    }
}
