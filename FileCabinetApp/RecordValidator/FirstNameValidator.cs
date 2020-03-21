using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class FirstNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        public FirstNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName), "Name is null.");
            }

            if (firstName.Length < this.minLength || firstName.Length > this.maxLength)
            {
                throw new ArgumentException($"{nameof(firstName)}'s length is less than {this.minLength} or more than {this.maxLength}.");
            }

            if (firstName.Contains(' ', StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"{nameof(firstName)} contains whitespaces.");
            }
        }
    }
}
