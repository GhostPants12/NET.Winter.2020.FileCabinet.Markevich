using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class LastNameValidator : IRecordValidator
    {
        public LastNameValidator() { }

        public LastNameValidator(int min, int max)
        {
            this.MinLength = min;
            this.MaxLength = max;
        }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

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
