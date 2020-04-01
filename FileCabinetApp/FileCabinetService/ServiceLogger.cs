using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp.FileCabinetService
{
    public class ServiceLogger : IFileCabinetService
    {
        private IFileCabinetService service;

        private TextWriter writer;

        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service;
        }

        public IRecordValidator GetValidator()
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling GetValidator().");
            IRecordValidator recordValidator = this.service.GetValidator();
            this.writer.WriteLine($"{DateTime.Now} - GetValidator() returned {recordValidator}.");
            this.writer.Close();
            return recordValidator;
        }

        public int CreateRecord(RecordData newRecordData)
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling CreateRecord() with FirstName = '{newRecordData.FirstName}', " +
                                  $"LastName = '{newRecordData.LastName}', DateOfBirth = '{newRecordData.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}', " +
                                  $"Code = '{newRecordData.Code}', Letter = '{newRecordData.Letter}', Balance = '{newRecordData.Balance}'.");
            int returnValue = this.service.CreateRecord(newRecordData);
            this.writer.WriteLine($"{DateTime.Now} - CreateRecord() returned {returnValue}.");
            this.writer.Close();
            return returnValue;
        }

        public void EditRecord(RecordData newRecordData)
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling EditRecord() with FirstName = '{newRecordData.FirstName}', " +
                                  $"LastName = '{newRecordData.LastName}', DateOfBirth = '{newRecordData.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}', " +
                                  $"Code = '{newRecordData.Code}', Letter = '{newRecordData.Letter}', Balance = '{newRecordData.Balance}'.");
            this.service.EditRecord(newRecordData);
            this.writer.WriteLine($"{DateTime.Now} - EditRecord() was executed.");
            this.writer.Close();
        }

        public void DeleteRecord(int id)
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling DeleteRecord() with Id = '{id}'.");
            this.service.DeleteRecord(id);
            this.writer.WriteLine($"{DateTime.Now} - DeleteRecord() was executed.");
            this.writer.Close();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByFirstName() with FirstName = '{firstName}'.");
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.FindByFirstName(firstName);
            this.writer.WriteLine($"{DateTime.Now} - FindByFirstName() returned {returnCollection}.");
            this.writer.Close();
            return returnCollection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByLastName() with LastName = '{lastName}'.");
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.FindByFirstName(lastName);
            this.writer.WriteLine($"{DateTime.Now} - FindByLastName() returned {returnCollection}.");
            this.writer.Close();
            return returnCollection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling FindByDateOfBirth() with DateOfBirth = '{dateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}'.");
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.FindByDateOfBirth(dateTime);
            this.writer.WriteLine($"{DateTime.Now} - FindByDateOfBirth() returned {returnCollection}.");
            this.writer.Close();
            return returnCollection;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling GetRecords().");
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.GetRecords();
            this.writer.WriteLine($"{DateTime.Now} - GetRecords() returned {returnCollection}.");
            this.writer.Close();
            return returnCollection;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling MakeSnapshot().");
            FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();
            this.writer.WriteLine($"{DateTime.Now} - MakeSnapshot() returned {snapshot}.");
            this.writer.Close();
            return snapshot;
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling Restore() with Snapshot = '{snapshot}'.");
            this.service.Restore(snapshot);
            this.writer.WriteLine($"{DateTime.Now} - Restore() was executed.");
            this.writer.Close();
        }

        public int Purge()
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling Purge().");
            int returnValue = this.service.Purge();
            this.writer.WriteLine($"{DateTime.Now} - Purge() returned {returnValue}.");
            this.writer.Close();
            return returnValue;
        }

        public int GetStat()
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling GetStat().");
            int returnValue = this.service.GetStat();
            this.writer.WriteLine($"{DateTime.Now} - GetStat() returned {returnValue}.");
            this.writer.Close();
            return returnValue;
        }

        public int GetRemovedStat()
        {
            this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
            this.writer.WriteLine($"{DateTime.Now} - Calling GetRemovedStat().");
            int returnValue = this.service.GetRemovedStat();
            this.writer.WriteLine($"{DateTime.Now} - GetRemovedStat() returned {returnValue}.");
            this.writer.Close();
            return returnValue;
        }
    }
}
