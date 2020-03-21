using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class CustomCodeValidator : IRecordValidator
    {
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (code < 1)
            {
                throw new ArgumentException($"{nameof(code)} is less than one.");
            }
        }
    }
}
