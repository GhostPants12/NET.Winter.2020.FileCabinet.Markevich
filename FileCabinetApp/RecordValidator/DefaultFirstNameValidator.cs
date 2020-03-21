using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class DefaultFirstNameValidator : IRecordValidator
    {
        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName), "Name is null.");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException($"{nameof(firstName)}'s length is less than 2 or more than 60.");
            }

            if (firstName.Contains(' ', StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"{nameof(firstName)} contains whitespaces.");
            }
        }
    }
}
