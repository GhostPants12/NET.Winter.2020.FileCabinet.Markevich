using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class BalanceValidator : IRecordValidator
    {
        public BalanceValidator() { }

        public BalanceValidator(decimal min, decimal max)
        {
            this.Min = min;
            this.Max = max;
        }

        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance,
            DateTime dateOfBirth)
        {
            if (balance < this.Min || balance > this.Max)
            {
                throw new ArgumentException($"{nameof(balance)} it less than {this.Min} or more than {this.Max}.");
            }
        }
    }
}
