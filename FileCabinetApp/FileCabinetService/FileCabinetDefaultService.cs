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

        /// <summary>Creates the validator.</summary>
        /// <returns>Returns the validator.</returns>
        public override IRecordValidator.IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
