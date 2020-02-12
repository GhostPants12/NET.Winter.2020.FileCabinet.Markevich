using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Custom implementation for the FileCabinetService.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>Checks the parameters.</summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="code">The code.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <exception cref="ArgumentException">Thrown when one of the parameters is incorrect.</exception>
        public override void CheckParameters(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            this.CheckName(firstName);
            this.CheckName(lastName);
            if (dateOfBirth < new DateTime(1900, 1, 1) || dateOfBirth > DateTime.Today)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is earlier than 01-Jan-1950 or later than today.");
            }

            if (code < 1)
            {
                throw new ArgumentException($"{nameof(code)} is less than zero.");
            }

            if (letter == ' ')
            {
                throw new ArgumentException($"{nameof(letter)} can't be whitespace.");
            }

            if (balance < 0)
            {
                throw new ArgumentException($"{nameof(balance)} can't be negative.");
            }
        }

        /// <summary>Checks the name.</summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">name - Name is null.</exception>
        /// <exception cref="ArgumentException">name - Name's length is less than two or more than 60.</exception>
        private void CheckName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Name is null.");
            }

            if (name.Length < 2 || name.Length > 30)
            {
                throw new ArgumentException($"{nameof(name)}'s length is less than 2 or more than 30.");
            }
        }
    }
}
