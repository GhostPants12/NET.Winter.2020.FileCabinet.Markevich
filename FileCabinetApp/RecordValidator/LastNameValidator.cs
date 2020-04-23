using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Validator class for the last name.</summary>
    public class LastNameValidator : IRecordValidator
    {
        #pragma warning disable CA1303 // Do not pass literals as localized parameters

        /// <summary>Initializes a new instance of the <see cref="LastNameValidator" /> class.</summary>
        public LastNameValidator()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LastNameValidator" /> class.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public LastNameValidator(int min, int max)
        {
            this.MinLength = min;
            this.MaxLength = max;
        }

        /// <summary>Gets or sets the minimum length.</summary>
        /// <value>The minimum length.</value>
        public int MinLength { get; set; }

        /// <summary>Gets or sets the maximum length.</summary>
        /// <value>The maximum length.</value>
        public int MaxLength { get; set; }

        /// <summary>Validates the parameters.</summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="code">Code.</param>
        /// <param name="letter">Letter.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <exception cref="ArgumentNullException">lastName - Last Name is null.</exception>
        /// <exception cref="ArgumentException">Thrown when lastName is incorrect.</exception>
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName), "Last Name is null.");
            }

            if (lastName.Length < this.MinLength || lastName.Length > this.MaxLength)
            {
                throw new ArgumentException($"{nameof(lastName)}'s length is less than {this.MinLength} or more than {this.MaxLength}.");
            }

            if (lastName.Contains(' ', StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"{nameof(lastName)} contains whitespaces.");
            }
        }
    }
}
