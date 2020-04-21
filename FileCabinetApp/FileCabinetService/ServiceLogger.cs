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
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling GetValidator().");
                IRecordValidator recordValidator = this.service.GetValidator();
                this.writer.WriteLine($"{DateTime.Now} - GetValidator() returned {recordValidator}.");
                this.writer.Close();
                return recordValidator;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public int CreateRecord(RecordData newRecordData)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine(
                    $"{DateTime.Now} - Calling CreateRecord() with FirstName = '{newRecordData.FirstName}', " +
                    $"LastName = '{newRecordData.LastName}', DateOfBirth = '{newRecordData.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}', " +
                    $"Code = '{newRecordData.Code}', Letter = '{newRecordData.Letter}', Balance = '{newRecordData.Balance}'.");
                int returnValue = this.service.CreateRecord(newRecordData);
                this.writer.WriteLine($"{DateTime.Now} - CreateRecord() returned {returnValue}.");
                return returnValue;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public void EditRecord(RecordData newRecordData)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling EditRecord() with FirstName = '{newRecordData.FirstName}', " +
                                      $"LastName = '{newRecordData.LastName}', DateOfBirth = '{newRecordData.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}', " +
                                      $"Code = '{newRecordData.Code}', Letter = '{newRecordData.Letter}', Balance = '{newRecordData.Balance}'.");
                this.service.EditRecord(newRecordData);
                this.writer.WriteLine($"{DateTime.Now} - EditRecord() was executed.");
            }
            finally
            {
                this.writer.Close();
            }
        }

        public void DeleteRecord(int id)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling DeleteRecord() with Id = '{id}'.");
                this.service.DeleteRecord(id);
                this.writer.WriteLine($"{DateTime.Now} - DeleteRecord() was executed.");
            }
            finally
            {
                this.writer.Close();
            }
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling FindByFirstName() with FirstName = '{firstName}'.");
                IEnumerable<FileCabinetRecord> returnCollection = this.service.FindByFirstName(firstName);
                this.writer.WriteLine($"{DateTime.Now} - FindByFirstName() returned {returnCollection}.");
                return returnCollection;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling FindByLastName() with LastName = '{lastName}'.");
                IEnumerable<FileCabinetRecord> returnCollection = this.service.FindByLastName(lastName);
                this.writer.WriteLine($"{DateTime.Now} - FindByLastName() returned {returnCollection}.");
                return returnCollection;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine(
                    $"{DateTime.Now} - Calling FindByDateOfBirth() with DateOfBirth = '{dateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}'.");
                IEnumerable<FileCabinetRecord> returnCollection = this.service.FindByDateOfBirth(dateTime);
                this.writer.WriteLine($"{DateTime.Now} - FindByDateOfBirth() returned {returnCollection}.");
                return returnCollection;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling GetRecords().");
                ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.GetRecords();
                this.writer.WriteLine($"{DateTime.Now} - GetRecords() returned {returnCollection}.");
                return returnCollection;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling MakeSnapshot().");
                FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();
                this.writer.WriteLine($"{DateTime.Now} - MakeSnapshot() returned {snapshot}.");
                return snapshot;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling Restore() with Snapshot = '{snapshot}'.");
                this.service.Restore(snapshot);
                this.writer.WriteLine($"{DateTime.Now} - Restore() was executed.");
            }
            finally
            {
                this.writer.Close();
            }
        }

        public int Purge()
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling Purge().");
                int returnValue = this.service.Purge();
                this.writer.WriteLine($"{DateTime.Now} - Purge() returned {returnValue}.");
                return returnValue;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public int GetStat()
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling GetStat().");
                int returnValue = this.service.GetStat();
                this.writer.WriteLine($"{DateTime.Now} - GetStat() returned {returnValue}.");
                return returnValue;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public int GetRemovedStat()
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling GetRemovedStat().");
                int returnValue = this.service.GetRemovedStat();
                this.writer.WriteLine($"{DateTime.Now} - GetRemovedStat() returned {returnValue}.");
                return returnValue;
            }
            finally
            {
                this.writer.Close();
            }
        }

        public Dictionary<string, string> GetSelectDictionary()
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                this.writer.WriteLine($"{DateTime.Now} - Calling GetSelectDictionary().");
                Dictionary<string, string> dic = this.service.GetSelectDictionary();
                this.writer.WriteLine($"{DateTime.Now} - GetSelectDictionary() returned {dic}.");
                return dic;
            }
            finally
            {
                this.writer.Close();
            }
        }
    }
}
