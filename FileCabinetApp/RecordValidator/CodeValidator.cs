using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class CodeValidator : IRecordValidator
    {
        private short min;
        private short max;

        public CodeValidator(short min, short max)
        {
            this.min = min;
            this.max = max;
        }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (code < this.min || code > this.max)
            {
                throw new ArgumentException($"{nameof(code)} is less than {this.min} or more than {this.max}.");
            }
        }
    }
}
