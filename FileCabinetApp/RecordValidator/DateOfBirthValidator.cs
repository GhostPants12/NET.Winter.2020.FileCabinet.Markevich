using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Validator class for DateOfBirth.</summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        /// <summary>Initializes a new instance of the <see cref="DateOfBirthValidator" /> class.</summary>
        public DateOfBirthValidator()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DateOfBirthValidator" /> class.</summary>
        /// <param name="from">The minimum date.</param>
        /// <param name="to">The maximum date.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.From = from;
            this.To = to;
        }

        /// <summary>Gets or sets the minimum date value.</summary>
        /// <value>From.</value>
        public DateTime From { get; set; }

        /// <summary>Gets or sets the maximum date value.</summary>
        /// <value>To.</value>
        public DateTime To { get; set; }

        /// <summary>Validates the parameters.</summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="code">Code.</param>
        /// <param name="letter">Letter.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <exception cref="ArgumentException">Thrown when date of birth is incorrect.</exception>
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (dateOfBirth < this.From || dateOfBirth > this.To)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is earlier than {this.From.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)} or later than {this.To.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}.");
            }
        }
    }
}
