using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class DefaultBalanceValidator : IRecordValidator
    {
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance,
            DateTime dateOfBirth)
        {
            if (balance < 0)
            {
                throw new ArgumentException($"{nameof(balance)} can't be negative.");
            }
        }
    }
}
