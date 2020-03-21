using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class BalanceValidator : IRecordValidator
    {
        private decimal min;
        private decimal max;

        public BalanceValidator(decimal min, decimal max)
        {
            this.min = min;
            this.max = max;
        }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance,
            DateTime dateOfBirth)
        {
            if (balance < this.min || balance > this.max)
            {
                throw new ArgumentException($"{nameof(balance)} it less than {this.min} or more than {this.max}.");
            }
        }
    }
}
