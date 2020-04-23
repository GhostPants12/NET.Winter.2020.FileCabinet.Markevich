using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Class that validates the code.</summary>
    public class CodeValidator : IRecordValidator
    {
        /// <summary>Initializes a new instance of the <see cref="CodeValidator" /> class.</summary>
        public CodeValidator()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CodeValidator" /> class.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public CodeValidator(short min, short max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>  Gets or sets the minimum value for code..</summary>
        /// <value>The minimum value.</value>
        public short Min { get; set; }

        /// <summary>  Gets or sets the maximum value for code..</summary>
        /// <value>The maximum value.</value>
        public short Max { get; set; }

        /// <summary>Validates the parameters.</summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="code">Code.</param>
        /// <param name="letter">Letter.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <exception cref="ArgumentException">Throw when code is incorrect.</exception>
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (code < this.Min || code > this.Max)
            {
                throw new ArgumentException($"{nameof(code)} is less than {this.Min} or more than {this.Max}.");
            }
        }
    }
}
