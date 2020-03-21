using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class CustomLastNameValidator : IRecordValidator
    {
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName), "Name is null.");
            }

            if (lastName.Length < 2 || lastName.Length > 30)
            {
                throw new ArgumentException($"{nameof(lastName)}'s length is less than 2 or more than 30.");
            }
        }
    }
}
