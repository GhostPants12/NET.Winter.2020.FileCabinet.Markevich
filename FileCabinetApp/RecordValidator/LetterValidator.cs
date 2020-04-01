using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class LetterValidator : IRecordValidator
    {
        public LetterValidator() { }

        public LetterValidator(string unsupported)
        {
            this.Unsupported = unsupported;
        }

        public string Unsupported { get; set; }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (this.Unsupported.Contains(letter, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException($"{nameof(letter)} can't be one of these symbols: \"{this.Unsupported}\".");
            }
        }
    }
}
