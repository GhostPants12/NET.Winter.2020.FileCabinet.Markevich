using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Validator class for the first name.</summary>
    public class FirstNameValidator : IRecordValidator
    {
        #pragma warning disable CA1303 // Do not pass literals as localized parameters

        /// <summary>Initializes a new instance of the <see cref="FirstNameValidator" /> class.</summary>
        public FirstNameValidator()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FirstNameValidator" /> class.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public FirstNameValidator(int min, int max)
        {
            this.MinLength = min;
            this.MaxLength = max;
        }

        /// <summary>Gets or sets the minimum length.</summary>
        /// <value>The minimum length of the first name.</value>
        public int MinLength { get; set; }

        /// <summary>Gets or sets the maximum length.</summary>
        /// <value>The maximum length of the first name.</value>
        public int MaxLength { get; set; }

        /// <summary>Validates the parameters.</summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="code">Code.</param>
        /// <param name="letter">Letter.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <exception cref="ArgumentNullException">firstName - Name is null.</exception>
        /// <exception cref="ArgumentException">Thrown when first name is incorrect.</exception>
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName), "Name is null.");
            }

            if (firstName.Length < this.MinLength || firstName.Length > this.MaxLength)
            {
                throw new ArgumentException($"{nameof(firstName)}'s length is less than {this.MinLength} or more than {this.MaxLength}.");
            }

            if (firstName.Contains(' ', StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"{nameof(firstName)} contains whitespaces.");
            }
        }
    }
}
