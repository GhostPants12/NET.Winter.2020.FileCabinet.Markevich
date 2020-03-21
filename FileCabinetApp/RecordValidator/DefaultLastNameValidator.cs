using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class DefaultLastNameValidator : IRecordValidator
    {
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName), "Last Name is null.");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException($"{nameof(lastName)}'s length is less than 2 or more than 60.");
            }

            if (lastName.Contains(' ', StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"{nameof(lastName)} contains whitespaces.");
            }
        }
    }
}
