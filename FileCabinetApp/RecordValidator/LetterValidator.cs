using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class LetterValidator : IRecordValidator
    {
        private string unsupported;

        public LetterValidator(string unsupported)
        {
            this.unsupported = unsupported;
        }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (this.unsupported.Contains(letter, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException($"{nameof(letter)} can't be one of these symbols: \"{this.unsupported}\".");
            }
        }
    }
}
