using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Validator for default file cabinet service.</summary>
    /// <seealso cref="FileCabinetApp.RecordValidator.IRecordValidator" />
    public class DefaultValidator : IRecordValidator
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
            new FirstNameValidator(2, 60).Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new LastNameValidator(2, 60).Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Today).Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new CodeValidator(0, short.MaxValue).Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new LetterValidator(" ").Validate(firstName, lastName, code, letter, balance, dateOfBirth);
            new BalanceValidator(0, decimal.MaxValue).Validate(firstName, lastName, code, letter, balance, dateOfBirth);
        }
    }
}
