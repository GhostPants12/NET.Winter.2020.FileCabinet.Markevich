using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Class-validator fo validating balance.</summary>
    public class BalanceValidator : IRecordValidator
    {
        /// <summary>Initializes a new instance of the <see cref="BalanceValidator" /> class.</summary>
        public BalanceValidator()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BalanceValidator" /> class.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public BalanceValidator(decimal min, decimal max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>Gets or sets the minimum value for the balance.</summary>
        /// <value>The minimum value.</value>
        public decimal Min { get; set; }

        /// <summary>Gets or sets the maximum value for the balance.</summary>
        /// <value>The maximum value.</value>
        public decimal Max { get; set; }

        /// <summary>Validates the parameters.</summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="code">Code.</param>
        /// <param name="letter">Letter.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <exception cref="ArgumentException">Thrown when balance is incorrect.</exception>
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (balance < this.Min || balance > this.Max)
            {
                throw new ArgumentException($"{nameof(balance)} it less than {this.Min} or more than {this.Max}.");
            }
        }
    }
}
