using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            this.CheckParameters(firstName, lastName, code, letter, balance, dateOfBirth);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                Code = code,
                Letter = letter,
                Balance = balance,
                DateOfBirth = dateOfBirth,
            };

            this.list.Add(record);

            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            this.CheckParameters(firstName, lastName, code, letter, balance, dateOfBirth);
            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.Code = code;
                    record.Letter = letter;
                    record.Balance = balance;
                    record.DateOfBirth = dateOfBirth;
                    return;
                }
            }

            throw new ArgumentException($"{nameof(id)} is incorrect.");
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> resultList = new List<FileCabinetRecord>();
            foreach (var record in this.list)
            {
                if (record.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    resultList.Add(record);
                }
            }

            return resultList.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> resultList = new List<FileCabinetRecord>();
            foreach (var record in this.list)
            {
                if (record.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    resultList.Add(record);
                }
            }

            return resultList.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateTime)
        {
            List<FileCabinetRecord> resultList = new List<FileCabinetRecord>();
            foreach (var record in this.list)
            {
                if (record.DateOfBirth.Equals(dateTime))
                {
                    resultList.Add(record);
                }
            }

            return resultList.ToArray();
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        private void CheckParameters(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            this.CheckName(firstName);
            this.CheckName(lastName);
            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Today)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is earlier than 01-Jan-1950 or later than today.");
            }

            if (code < 0)
            {
                throw new ArgumentException($"{nameof(code)} is less than zero.");
            }

            if (letter == ' ')
            {
                throw new ArgumentException($"{nameof(letter)} can't be whitespace.");
            }

            if (balance < 0)
            {
                throw new ArgumentException($"{nameof(balance)} can't be negative.");
            }
        }

        private void CheckName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Name is null.");
            }

            if (name.Length < 2 || name.Length > 60)
            {
                throw new ArgumentException($"{nameof(name)}'s length is less than zero or more than 60.");
            }

            if (name.Contains(' ', StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"{nameof(name)} contains whitespaces.");
            }
        }
    }
}
