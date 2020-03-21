using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Interface for fields' validation.</summary>
    public interface IRecordValidator
    {
        /// <summary>Validates the parameters.</summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="code">Code.</param>
        /// <param name="letter">Letter.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth);
    }
}
