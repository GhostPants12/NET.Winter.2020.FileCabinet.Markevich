using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Validator class for letter.</summary>
    public class LetterValidator : IRecordValidator
    {
        /// <summary>Initializes a new instance of the <see cref="LetterValidator" /> class.</summary>
        public LetterValidator()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LetterValidator" /> class.</summary>
        /// <param name="unsupported">The unsupported symbols.</param>
        public LetterValidator(string unsupported)
        {
            this.Unsupported = unsupported;
        }

        /// <summary>Gets or sets the unsupported symbols.</summary>
        /// <value>The unsupported symbols.</value>
        public string Unsupported { get; set; }

        /// <summary>Validates the parameters.</summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="code">Code.</param>
        /// <param name="letter">Letter.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <exception cref="ArgumentException">Throw when letter is incorrect.</exception>
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (this.Unsupported.Contains(letter, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException($"{nameof(letter)} can't be one of these symbols: \"{this.Unsupported}\".");
            }
        }
    }
}
