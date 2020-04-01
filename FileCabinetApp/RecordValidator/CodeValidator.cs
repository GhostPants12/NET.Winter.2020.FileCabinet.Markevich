using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class CodeValidator : IRecordValidator
    {
        public CodeValidator() { }

        public CodeValidator(short min, short max)
        {
            this.Min = min;
            this.Max = max;
        }

        public short Min { get; set; }

        public short Max { get; set; }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (code < this.Min || code > this.Max)
            {
                throw new ArgumentException($"{nameof(code)} is less than {this.Min} or more than {this.Max}.");
            }
        }
    }
}
