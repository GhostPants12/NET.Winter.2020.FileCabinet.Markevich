using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class LastNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        public LastNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName), "Last Name is null.");
            }

            if (lastName.Length < this.minLength || lastName.Length > this.maxLength)
            {
                throw new ArgumentException($"{nameof(lastName)}'s length is less than {this.minLength} or more than {this.maxLength}.");
            }

            if (lastName.Contains(' ', StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"{nameof(lastName)} contains whitespaces.");
            }
        }
    }
}
