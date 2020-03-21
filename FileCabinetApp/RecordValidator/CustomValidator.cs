using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Validator for custom file cabinet.</summary>
    /// <seealso cref="FileCabinetApp.RecordValidator.IRecordValidator" />
    public class CustomValidator : IRecordValidator
    {
        /// <summary>Checks the parameters.</summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="code">The code.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <exception cref="ArgumentException">Thrown when one of the parameters is incorrect.</exception>
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            new CustomFirstNameValidator().Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new CustomLastNameValidator().Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new CustomDateOfBirthValidator().Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new CustomCodeValidator().Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new CustomLetterValidator().Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new CustomBalanceValidator().Validate(firstName, lastName, code, letter, balance, dateOfBirth);
        }
    }
}
