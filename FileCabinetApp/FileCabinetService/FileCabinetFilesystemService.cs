using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Iterators;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp
{
    /// <summary>FileCabinetService that contains records in a binary file.</summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly IRecordValidator validator;
        private readonly SortedList<string, List<long>> firstNameDictionary =
            new SortedList<string, List<long>>();

        private readonly SortedList<string, List<long>> lastNameDictionary =
            new SortedList<string, List<long>>();

        private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary =
            new Dictionary<DateTime, List<long>>();

        private Dictionary<string, string> selectDictionary =
            new Dictionary<string, string>();

        private int removedCount;
        private int count;
        private int identifiersCount;
        private FileStream fileStream;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetFilesystemService" /> class.</summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="validator">The validator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

        /// <summary>Creates the record.</summary>
        /// <param name="newRecordData">The new record data.</param>
        /// <returns>Id of the record.</returns>
        public int CreateRecord(RecordData newRecordData)
        {
            if (newRecordData != null)
            {
                this.validator.Validate(newRecordData.FirstName, newRecordData.LastName, newRecordData.Code, newRecordData.Letter, newRecordData.Balance, newRecordData.DateOfBirth);
                if (!this.firstNameDictionary.ContainsKey(newRecordData.FirstName))
                {
                    this.firstNameDictionary.Add(newRecordData.FirstName, new List<long>());
                    this.firstNameDictionary[newRecordData.FirstName].Add(this.fileStream.Position);
                }
                else
                {
                    this.firstNameDictionary[newRecordData.FirstName].Add(this.fileStream.Position);
                }

                if (!this.lastNameDictionary.ContainsKey(newRecordData.LastName))
                {
                    this.lastNameDictionary.Add(newRecordData.LastName, new List<long>());
                    this.lastNameDictionary[newRecordData.LastName].Add(this.fileStream.Position);
                }
                else
                {
                    this.lastNameDictionary[newRecordData.LastName].Add(this.fileStream.Position);
                }

                if (!this.dateOfBirthDictionary.ContainsKey(newRecordData.DateOfBirth))
                {
                    this.dateOfBirthDictionary.Add(newRecordData.DateOfBirth, new List<long>());
                    this.dateOfBirthDictionary[newRecordData.DateOfBirth].Add(this.fileStream.Position);
                }
                else
                {
                    this.dateOfBirthDictionary[newRecordData.DateOfBirth].Add(this.fileStream.Position);
                }

                newRecordData.Id = ++this.identifiersCount;
                byte[] buffer = new byte[120];
                byte[] cleanArr = new byte[120];
                this.fileStream.Write(BitConverter.GetBytes((short)0), 0, 2);
                int i = 0;
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.Id));
                foreach (var element in Encoding.Default.GetBytes(newRecordData.FirstName))
                {
                    buffer[i] = element;
                    i++;
                }

                i = 0;
                this.fileStream.Write(buffer, 0, 120);
                Array.Copy(cleanArr, buffer, 120);
                foreach (var element in Encoding.Default.GetBytes(newRecordData.LastName))
                {
                    buffer[i] = element;
                    i++;
                }

                this.fileStream.Write(buffer, 0, 120);
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.DateOfBirth.Year));
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.DateOfBirth.Month));
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.DateOfBirth.Day));
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.Code));
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.Letter));
                foreach (int value in decimal.GetBits(newRecordData.Balance))
                {
                    this.fileStream.Write(BitConverter.GetBytes(value));
                }

                this.selectDictionary = new Dictionary<string, string>();
                return newRecordData.Id;
            }

            #pragma warning disable CA1303 // Do not pass literals as localized parameters
            throw new ArgumentException($"{nameof(newRecordData)} is incorrect.");
            #pragma warning restore CA1303 // Do not pass literals as localized parameters
        }

        /// <summary>Edits the record.</summary>
        /// <param name="newRecordData">The new record data.</param>
        public void EditRecord(RecordData newRecordData)
        {
            if (newRecordData != null)
            {
                this.validator.Validate(newRecordData.FirstName, newRecordData.LastName, newRecordData.Code, newRecordData.Letter, newRecordData.Balance, newRecordData.DateOfBirth);
                foreach (var record in this.GetRecords())
                {
                    if (record.Id == newRecordData.Id)
                    {
                        if (this.firstNameDictionary[record.FirstName].Count > 1)
                        {
                            this.firstNameDictionary[record.FirstName].Remove(this.fileStream.Position);
                        }
                        else
                        {
                            this.firstNameDictionary.Remove(record.FirstName);
                        }

                        if (this.lastNameDictionary[record.LastName].Count > 1)
                        {
                            this.lastNameDictionary[record.LastName].Remove(this.fileStream.Position);
                        }
                        else
                        {
                            this.lastNameDictionary.Remove(record.LastName);
                        }

                        if (this.dateOfBirthDictionary[record.DateOfBirth].Count > 1)
                        {
                            this.dateOfBirthDictionary[record.DateOfBirth].Remove(this.fileStream.Position);
                        }
                        else
                        {
                            this.dateOfBirthDictionary.Remove(record.DateOfBirth);
                        }
                    }
                }

                long positionBackup = this.fileStream.Position;
                int i = 0;
                this.fileStream.Position = 0;
                byte[] cleanArr = new byte[280];
                byte[] buffer = new byte[280];
                this.SetPositionToId(newRecordData.Id);

                if (!this.firstNameDictionary.ContainsKey(newRecordData.FirstName))
                {
                    this.firstNameDictionary.Add(newRecordData.FirstName, new List<long>());
                    this.firstNameDictionary[newRecordData.FirstName].Add(this.fileStream.Position);
                }
                else
                {
                    this.firstNameDictionary[newRecordData.FirstName].Add(this.fileStream.Position);
                }

                if (!this.lastNameDictionary.ContainsKey(newRecordData.LastName))
                {
                    this.lastNameDictionary.Add(newRecordData.LastName, new List<long>());
                    this.lastNameDictionary[newRecordData.LastName].Add(this.fileStream.Position);
                }
                else
                {
                    this.lastNameDictionary[newRecordData.LastName].Add(this.fileStream.Position);
                }

                if (!this.dateOfBirthDictionary.ContainsKey(newRecordData.DateOfBirth))
                {
                    this.dateOfBirthDictionary.Add(newRecordData.DateOfBirth, new List<long>());
                    this.dateOfBirthDictionary[newRecordData.DateOfBirth].Add(this.fileStream.Position);
                }
                else
                {
                    this.dateOfBirthDictionary[newRecordData.DateOfBirth].Add(this.fileStream.Position);
                }

                foreach (var element in Encoding.Default.GetBytes(newRecordData.FirstName))
                {
                    buffer[i] = element;
                    i++;
                }

                i = 0;
                this.fileStream.Write(buffer, 0, 120);
                Array.Copy(cleanArr, buffer, 120);
                foreach (var element in Encoding.Default.GetBytes(newRecordData.LastName))
                {
                    buffer[i] = element;
                    i++;
                }

                this.fileStream.Write(buffer, 0, 120);
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.DateOfBirth.Year));
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.DateOfBirth.Month));
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.DateOfBirth.Day));
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.Code));
                this.fileStream.Write(BitConverter.GetBytes(newRecordData.Letter));
                foreach (int value in decimal.GetBits(newRecordData.Balance))
                {
                    this.fileStream.Write(BitConverter.GetBytes(value));
                }

                this.fileStream.Position = positionBackup;
            }

            this.selectDictionary = new Dictionary<string, string>();
        }

        /// <summary>Finds the record by date of birth.</summary>
        /// <param name="dateTime">The date of birth.</param>
        /// <returns>Records that contain such dateOfBirth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            foreach (var element in new FilesystemCollection(this.fileStream, !this.dateOfBirthDictionary.ContainsKey(dateTime) ? new List<long>() : this.dateOfBirthDictionary[dateTime]))
            {
                yield return element;
            }
        }

        /// <summary>Finds the record by first name.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Records with such first name.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            foreach (var element in new FilesystemCollection(this.fileStream, !this.firstNameDictionary.ContainsKey(firstName) ? new List<long>() : this.firstNameDictionary[firstName]))
            {
                yield return element;
            }
        }

        /// <summary>Finds the record by last name.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Records with such last name.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            foreach (var element in new FilesystemCollection(this.fileStream, !this.lastNameDictionary.ContainsKey(lastName) ? new List<long>() : this.lastNameDictionary[lastName]))
            {
                yield return element;
            }
        }

        /// <summary>Gets the records.</summary>
        /// <returns>Collection of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.removedCount = 0;
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            int year, month, day;
            int[] decimalArray = new int[4];
            byte[] bytes = new byte[120];
            this.fileStream.Position = 0;
            while (this.fileStream.Position < this.fileStream.Length)
            {
                FileCabinetRecord recordToAdd = new FileCabinetRecord();
                this.fileStream.Read(bytes, 0, 2);
                if ((bytes[0] & 4) == 4)
                {
                    this.fileStream.Position += 276;
                    this.removedCount++;
                    continue;
                }

                this.fileStream.Read(bytes, 0, 4);
                recordToAdd.Id = BitConverter.ToInt32(bytes);
                this.fileStream.Read(bytes, 0, 120);
                recordToAdd.FirstName = Encoding.Default.GetString(bytes, 0, 120).Replace("\0", string.Empty, StringComparison.InvariantCulture);
                this.fileStream.Read(bytes, 0, 120);
                recordToAdd.LastName = Encoding.Default.GetString(bytes, 0, 120).Replace("\0", string.Empty, StringComparison.InvariantCulture);
                this.fileStream.Read(bytes, 0, 4);
                year = BitConverter.ToInt32(bytes);
                this.fileStream.Read(bytes, 0, 4);
                month = BitConverter.ToInt32(bytes);
                this.fileStream.Read(bytes, 0, 4);
                day = BitConverter.ToInt32(bytes);
                recordToAdd.DateOfBirth = new DateTime(year, month, day);
                this.fileStream.Read(bytes, 0, 2);
                recordToAdd.Code = BitConverter.ToInt16(bytes);
                this.fileStream.Read(bytes, 0, 2);
                recordToAdd.Letter = BitConverter.ToChar(bytes);
                for (int i = 0; i < decimalArray.Length; i++)
                {
                    this.fileStream.Read(bytes, 0, 4);
                    decimalArray[i] = BitConverter.ToInt32(bytes);
                }

                recordToAdd.Balance = new decimal(decimalArray);
                records.Add(recordToAdd);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>Purges this instance.</summary>
        /// <returns>Returns the amount of purged elements.</returns>
        public int Purge()
        {
            int purgedElements = 0;
            byte[] deletionBuf = new byte[2];
            this.fileStream.Position = 0;
            do
            {
                this.fileStream.Read(deletionBuf, 0, 2);
                if ((deletionBuf[0] & 4) == 4)
                {
                    this.fileStream.Position -= 2;
                    long positionBackup = this.fileStream.Position;
                    this.fileStream.Position += 278;
                    if (this.fileStream.Length - this.fileStream.Position > 0)
                    {
                        byte[] buf = new byte[this.fileStream.Length - this.fileStream.Position];
                        this.fileStream.Read(buf, 0, buf.Length);
                        this.fileStream.Position = positionBackup;
                        this.fileStream.Write(buf, 0, buf.Length);
                        this.fileStream.SetLength(this.fileStream.Position);
                        this.fileStream.Position = positionBackup;
                        purgedElements++;
                        continue;
                    }
                    else
                    {
                        this.fileStream.Position -= 278;
                        purgedElements++;
                        this.fileStream.SetLength(this.fileStream.Position);
                        break;
                    }
                }

                this.fileStream.Position += 276;
            }
            while (this.fileStream.Position != this.fileStream.Length);

            this.removedCount = 0;
            return purgedElements;
        }

        /// <summary>Gets the stat.</summary>
        /// <returns>Count of the records in this FileCabinet.</returns>
        public int GetStat()
        {
            this.count = (int)(this.fileStream.Length / 278);
            return this.count;
        }

        /// <summary>Gets the validator.</summary>
        /// <returns>Returns the validator.</returns>
        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        /// <summary>Makes the snapshot.</summary>
        /// <returns>Returns the snapshot of current FileCabinet.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetRecord[] records = new FileCabinetRecord[this.GetRecords().Count];
            this.GetRecords().CopyTo(records, 0);
            return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>(records));
        }

        /// <summary>Restores the FileCabinet using specified snapshot.</summary>
        /// <param name="snapshot">The snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.fileStream.Position = 0;
            if (snapshot != null)
            {
                foreach (var element in snapshot.Records)
                {
                    this.count = element.Id--;
                    this.CreateRecord(new RecordData(element.FirstName, element.LastName, element.Code, element.Letter, element.Balance, element.DateOfBirth));
                }
            }
        }

        /// <summary>Deletes the record with specified id.</summary>
        /// <param name="id">The identifier.</param>
        public void DeleteRecord(int id)
        {
            byte[] buf = new byte[2];
            this.SetPositionToId(id);
            this.fileStream.Position -= 6;
            this.fileStream.Read(buf, 0, 2);
            this.fileStream.Position -= 2;
            this.fileStream.Write(new byte[] { (byte)(buf[0] | 4),  buf[1] }, 0, 2);
            this.selectDictionary = new Dictionary<string, string>();
        }

        /// <summary>Gets the removed stat.</summary>
        /// <returns>Amount of removed records.</returns>
        public int GetRemovedStat()
        {
            this.GetRecords();
            return this.removedCount;
        }

        /// <summary>Gets the select dictionary.</summary>
        /// <returns>Returns the select dictionary of this FileCabinet.</returns>
        public Dictionary<string, string> GetSelectDictionary()
        {
            return this.selectDictionary;
        }

        /// <summary>Sets file position to specified id.</summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="ArgumentException">Id #{id} is incorrect.</exception>
        private void SetPositionToId(int id)
        {
            this.fileStream.Position = 0;
            byte[] buffer = new byte[4];

            do
            {
                this.fileStream.Read(buffer, 0, 2);
                if ((buffer[0] & 4) == 4)
                {
                    this.fileStream.Position += 276;
                    continue;
                }

                this.fileStream.Read(buffer, 0, 4);
                if (BitConverter.ToInt32(buffer) == id)
                {
                    return;
                }

                this.fileStream.Position += 272;
            }
            while (this.fileStream.Position != this.fileStream.Length);

            throw new ArgumentException($"Id #{id} is incorrect.");
        }
    }
}
