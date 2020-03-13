using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.IRecordValidator;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private int count = 0;
        private FileStream fileStream;
        private readonly IRecordValidator.IRecordValidator validator;

        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator.IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

        public int CreateRecord(RecordData newRecordData)
        {
            this.validator.ValidateParameters(newRecordData.FirstName, newRecordData.LastName, newRecordData.Code, newRecordData.Letter, newRecordData.Balance, newRecordData.DateOfBirth);
            newRecordData.Id = ++this.count;
            byte[] buffer = new byte[120];
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
            int i = 0;
            byte[] buffer = new byte[120];
            foreach (var record in this.GetRecords())
            {
                if (record.Id == newRecordData.Id)
                {
                    this.fileStream.Position = ((newRecordData.Id - 1) * 276) + 4;
                    this.validator.ValidateParameters(newRecordData.FirstName, newRecordData.LastName,
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

                    return;
                }
            }

            throw new ArgumentException($"{nameof(newRecordData.Id)} is incorrect.");
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            List<FileCabinetRecord> returnList = new List<FileCabinetRecord>();
            foreach (var record in this.GetRecords())
            {
                if (record.DateOfBirth.Equals(dateTime))
                {
                    returnList.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(returnList);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> returnList = new List<FileCabinetRecord>();
            foreach (var record in this.GetRecords())
            {
                if (record.FirstName.Equals(firstName, StringComparison.InvariantCulture))
                {
                    returnList.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(returnList);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> returnList = new List<FileCabinetRecord>();
            foreach (var record in this.GetRecords())
            {
                if (record.LastName.Equals(lastName, StringComparison.InvariantCulture))
                {
                    returnList.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(returnList);
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            int year, month, day;
            int[] decimalArray = new int[4];
            byte[] bytes = new byte[120];
            this.fileStream.Position = 0;
            while (this.fileStream.Position < this.fileStream.Length)
            {
                FileCabinetRecord recordToAdd = new FileCabinetRecord();
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

        public int GetStat()
        {
            return this.count;
        }

        public IRecordValidator.IRecordValidator GetValidator()
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
    }
}
