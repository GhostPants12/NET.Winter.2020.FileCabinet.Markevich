using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.IRecordValidator
{
    /// <summary>Validator for custom file cabinet.</summary>
    /// <seealso cref="FileCabinetApp.IRecordValidator.IRecordValidator" />
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
        public void ValidateParameters(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            this.ValidateFirstName(firstName);
            this.ValidateLastName(lastName);
            this.ValidateDateOfBirth(dateOfBirth);
            this.ValidateCode(code);
            this.ValidateLetter(letter);
            this.ValidateBalance(balance);
        }

        /// <summary>Validates the first name.</summary>
        /// <param name="firstName">The name.</param>
        /// <exception cref="ArgumentNullException">name - Name is null.</exception>
        /// <exception cref="ArgumentException">name - Name's length is less than two or more than 60.</exception>
        private void ValidateFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName), "Name is null.");
            }

            if (firstName.Length < 2 || firstName.Length > 30)
            {
                throw new ArgumentException($"{nameof(firstName)}'s length is less than 2 or more than 30.");
            }
        }

        /// <summary>Checks the name.</summary>
        /// <param name="lastName">The name.</param>
        /// <exception cref="ArgumentNullException">name - Name is null.</exception>
        /// <exception cref="ArgumentException">name - Name's length is less than two or more than 60.</exception>
        private void ValidateLastName(string lastName)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName), "Name is null.");
            }

            if (lastName.Length < 2 || lastName.Length > 30)
            {
                throw new ArgumentException($"{nameof(lastName)}'s length is less than 2 or more than 30.");
            }
        }

        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < new DateTime(1900, 1, 1) || dateOfBirth > DateTime.Today)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is earlier than 01-Jan-1950 or later than today.");
            }
        }

        private void ValidateCode(short code)
        {
            if (code < 1)
            {
                throw new ArgumentException($"{nameof(code)} is less than one.");
            }
        }

        private void ValidateLetter(char letter)
        {
            if (letter == ' ')
            {
                throw new ArgumentException($"{nameof(letter)} can't be whitespace.");
            }
        }

        private void ValidateBalance(decimal balance)
        {
            if (balance < 0)
            {
                throw new ArgumentException($"{nameof(balance)} can't be negative.");
            }
        }
    }
}
