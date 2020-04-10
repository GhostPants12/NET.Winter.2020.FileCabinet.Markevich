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
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private int removedCount;
        private int count;
        private int idCounter;
        private FileStream fileStream;
        private readonly IRecordValidator validator;
        private readonly SortedList<string, List<long>> firstNameDictionary =
            new SortedList<string, List<long>>();

        private readonly SortedList<string, List<long>> lastNameDictionary =
            new SortedList<string, List<long>>();

        private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary =
            new Dictionary<DateTime, List<long>>();

        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

        public int CreateRecord(RecordData newRecordData)
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

            newRecordData.Id = ++this.idCounter;
            byte[] buffer = new byte[120];
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

            return newRecordData.Id;
        }

        public void EditRecord(RecordData newRecordData)
        {
            long positionBackup = this.fileStream.Position;
            int i = 0;
            this.fileStream.Position = 0;
            byte[] buffer = new byte[280];
            this.SetPositionToId(newRecordData.Id);
            this.validator.Validate(newRecordData.FirstName, newRecordData.LastName,
                newRecordData.Code, newRecordData.Letter, newRecordData.Balance, newRecordData.DateOfBirth);
            foreach (var element in Encoding.Default.GetBytes(newRecordData.FirstName))
            {
                buffer[i] = element;
                i++;
            }

            i = 0;
            this.fileStream.Write(buffer, 0, 120);
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

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime) => new FilesystemCollection(this.fileStream, !this.dateOfBirthDictionary.ContainsKey(dateTime) ? new List<long>() : this.dateOfBirthDictionary[dateTime]);

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName) => new FilesystemCollection(this.fileStream, !this.firstNameDictionary.ContainsKey(firstName) ? new List<long>() : this.firstNameDictionary[firstName]);

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName) => new FilesystemCollection(this.fileStream, !this.lastNameDictionary.ContainsKey(lastName) ? new List<long>() : this.lastNameDictionary[lastName]);

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

        public int GetStat()
        {
            this.count = (int)(this.fileStream.Length / 278);
            return this.count;
        }

        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetRecord[] records = new FileCabinetRecord[this.GetRecords().Count];
            this.GetRecords().CopyTo(records, 0);
            return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>(records));
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.fileStream.Position = 0;
            foreach (var element in snapshot.Records)
            {
                this.count = element.Id--;
                CreateRecord(new RecordData(element.FirstName, element.LastName, element.Code, element.Letter, element.Balance, element.DateOfBirth));
            }
        }

        public void DeleteRecord(int id)
        {
            byte[] buf = new byte[2];
            this.SetPositionToId(id);
            this.fileStream.Position -= 6;
            this.fileStream.Read(buf, 0, 2);
            this.fileStream.Position -= 2;
            this.fileStream.Write(new byte[] { (byte)(buf[0] | 4),  buf[1] }, 0, 2);
        }

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

        public int GetRemovedStat()
        {
            this.GetRecords();
            return this.removedCount;
        }
    }
}
