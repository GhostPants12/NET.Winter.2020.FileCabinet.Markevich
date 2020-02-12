using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Class for working with the file cabinet.</summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>Creates the record.</summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="code">The code.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>Returns the new record's ID.</returns>
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
            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
                this.firstNameDictionary[firstName].Add(record);
            }
            else
            {
                this.firstNameDictionary[firstName].Add(record);
            }

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>());
                this.lastNameDictionary[lastName].Add(record);
            }
            else
            {
                this.lastNameDictionary[lastName].Add(record);
            }

            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<FileCabinetRecord>());
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }

            return record.Id;
        }

        /// <summary>Edits the record.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="code">The code.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <exception cref="ArgumentException">Thrown when id is incorrect.</exception>
        public void EditRecord(int id, string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            this.CheckParameters(firstName, lastName, code, letter, balance, dateOfBirth);
            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.firstNameDictionary.Remove(record.FirstName);
                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.Code = code;
                    record.Letter = letter;
                    record.Balance = balance;
                    record.DateOfBirth = dateOfBirth;
                    if (!this.firstNameDictionary.ContainsKey(firstName))
                    {
                        this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
                        this.firstNameDictionary[firstName].Add(record);
                    }
                    else
                    {
                        this.firstNameDictionary[firstName].Add(record);
                    }

                    return;
                }
            }

            throw new ArgumentException($"{nameof(id)} is incorrect.");
        }

        /// <summary>Finds the record by its first name.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>The array of record with specific first name.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> resultList = new List<FileCabinetRecord>();
            foreach (var key in this.firstNameDictionary.Keys)
            {
                if (firstName.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    resultList = this.firstNameDictionary[key];
                }
            }

            return resultList.ToArray();
        }

        /// <summary>Finds the record by its last name.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>The array of record with specific last name.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> resultList = new List<FileCabinetRecord>();
            foreach (var key in this.lastNameDictionary.Keys)
            {
                if (lastName.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    resultList = this.lastNameDictionary[key];
                }
            }

            return resultList.ToArray();
        }

        /// <summary>Finds the record by its date of birth.</summary>
        /// <param name="dateTimee">The date of birth.</param>
        /// <returns>The array of record with specific date of birth.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateTime)
        {
            List<FileCabinetRecord> resultList = new List<FileCabinetRecord>();
            foreach (var key in this.dateOfBirthDictionary.Keys)
            {
                if (dateTime.Equals(key))
                {
                    resultList = this.dateOfBirthDictionary[key];
                }
            }

            return resultList.ToArray();
        }

        /// <summary>Gets all the records.</summary>
        /// <returns>An array of records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>Gets the stat.</summary>
        /// <returns>The number of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>Checks the parameters.</summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="code">The code.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <exception cref="ArgumentException">Thrown when one of the parameters is incorrect.</exception>
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

        /// <summary>Checks the name.</summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">name - Name is null.</exception>
        /// <exception cref="ArgumentException">name - Name's length is less than two or more than 60.</exception>
        private void CheckName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Name is null.");
            }

            if (name.Length < 2 || name.Length > 60)
            {
                throw new ArgumentException($"{nameof(name)}'s length is less than 2 or more than 60.");
            }

            if (name.Contains(' ', StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"{nameof(name)} contains whitespaces.");
            }
        }
    }
}
