using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp.RecordValidator
{
    public class DateOfBirthValidator : IRecordValidator
    {
        public DateOfBirthValidator() { }

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.From = from;
            this.To = to;
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public void Validate(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            if (dateOfBirth < this.From || dateOfBirth > this.To)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is earlier than {this.From.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)} or later than {this.To.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}.");
            }
        }
    }
}
