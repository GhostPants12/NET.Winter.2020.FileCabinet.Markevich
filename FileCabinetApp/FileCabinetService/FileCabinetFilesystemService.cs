using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            using (BinaryWriter br = new BinaryWriter(this.fileStream))
            {
                br.Write(newRecordData.Id);
                foreach (var element in Encoding.Default.GetBytes(newRecordData.FirstName))
                {
                    buffer[i] = element;
                    i++;
                }

                i = 0;
                br.Write(buffer, 0, 120);
                foreach (var element in Encoding.Default.GetBytes(newRecordData.LastName))
                {
                    buffer[i] = element;
                    i++;
                }

                br.Write(buffer, 0, 120);
                br.Write(newRecordData.DateOfBirth.Year);
                br.Write(newRecordData.DateOfBirth.Month);
                br.Write(newRecordData.DateOfBirth.Day);
                br.Write(newRecordData.Code);
                br.Write(newRecordData.Letter);
                br.Write(newRecordData.Balance);
            }

            return newRecordData.Id;
        }

        public void EditRecord(RecordData newRecordData)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public IRecordValidator.IRecordValidator GetValidator()
        {
            return this.validator;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
