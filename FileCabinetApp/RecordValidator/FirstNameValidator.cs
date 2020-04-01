using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class FirstNameValidator : IRecordValidator
    {
        public FirstNameValidator() { }

        public FirstNameValidator(int min, int max)
        {
            this.MinLength = min;
            this.MaxLength = max;
        }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

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
