using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private DateTime from;
        private DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (dateOfBirth < this.from || dateOfBirth > this.to)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is earlier than {this.from.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)} or later than {this.to.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}.");
            }
        }
    }
}
