using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp.FileCabinetService
{
    /// <summary>Class-decorator for FileCabinet logging.</summary>
    public class ServiceLogger : IFileCabinetService, IDisposable
    {
        #pragma warning disable CA1303 // Do not pass literals as localized parameters

        private readonly IFileCabinetService service;

        private bool isDisposed;

        private TextWriter writer;

        /// <summary>Initializes a new instance of the <see cref="ServiceLogger" /> class.</summary>
        /// <param name="service">The service.</param>
        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>Gets the validator.</summary>
        /// <returns>Returns the validator.</returns>
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

        /// <summary>Creates the record.</summary>
        /// <param name="newRecordData">The new record data.</param>
        /// <returns>Returns the id of a created record.</returns>
        public int CreateRecord(RecordData newRecordData)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                if (newRecordData != null)
                {
                    this.writer.WriteLine(
                        $"{DateTime.Now} - Calling CreateRecord() with FirstName = '{newRecordData.FirstName}', " +
                        $"LastName = '{newRecordData.LastName}', DateOfBirth = '{newRecordData.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}', " +
                        $"Code = '{newRecordData.Code}', Letter = '{newRecordData.Letter}', Balance = '{newRecordData.Balance}'.");
                    int returnValue = this.service.CreateRecord(newRecordData);
                    this.writer.WriteLine($"{DateTime.Now} - CreateRecord() returned {returnValue}.");
                    return returnValue;
                }

                throw new ArgumentNullException(nameof(newRecordData), $"{nameof(newRecordData)} is null.");
            }
            finally
            {
                this.writer.Close();
            }
        }

        /// <summary>Edits the record.</summary>
        /// <param name="newRecordData">The new record data.</param>
        public void EditRecord(RecordData newRecordData)
        {
            try
            {
                this.writer = new StreamWriter(new FileStream("log.txt", FileMode.Append));
                if (newRecordData != null)
                {
                    this.writer.WriteLine(
                        $"{DateTime.Now} - Calling EditRecord() with FirstName = '{newRecordData.FirstName}', " +
                        $"LastName = '{newRecordData.LastName}', DateOfBirth = '{newRecordData.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}', " +
                        $"Code = '{newRecordData.Code}', Letter = '{newRecordData.Letter}', Balance = '{newRecordData.Balance}'.");
                    this.service.EditRecord(newRecordData);
                }

                this.writer.WriteLine($"{DateTime.Now} - EditRecord() was executed.");
            }
            finally
            {
                this.writer.Close();
            }
        }

        /// <summary>Deletes the record with the specified id.</summary>
        /// <param name="id">The identifier.</param>
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

        /// <summary>Finds the records by firstname.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Returns records with the specified first name.</returns>
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

        /// <summary>Finds the records by lastname.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Returns records with the specified last name.</returns>
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

        /// <summary>Finds the records by date of birth.</summary>
        /// <param name="dateTime">The date of birth.</param>
        /// <returns>Returns records with the specified date of birth.</returns>
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

        /// <summary>Gets the records.</summary>
        /// <returns>Returns the collection of records.</returns>
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

        /// <summary>Makes the snapshot.</summary>
        /// <returns>Returns the snapshot of current FileCabinet.</returns>
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

        /// <summary>Restores the FileCabinet with specified snapshot.</summary>
        /// <param name="snapshot">The snapshot.</param>
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

        /// <summary>Purges this instance.</summary>
        /// <returns>Amount of purged elements.</returns>
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

        /// <summary>Gets the stat.</summary>
        /// <returns>Returns the count of records.</returns>
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

        /// <summary>Gets the removed stat.</summary>
        /// <returns>Returns the count of removed records.</returns>
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

        /// <summary>Gets the select dictionary.</summary>
        /// <returns>Returns the select dictionary.</returns>
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

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <param name="disposing">If true, this object is going to be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.writer.Dispose();
            }

            this.isDisposed = true;
        }
    }
}
