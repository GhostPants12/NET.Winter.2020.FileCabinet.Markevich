using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp.FileCabinetService
{
    /// <summary>Class-decorator for recording and logging the time of FileCabinet methods.</summary>
    public class RecordMeter : IFileCabinetService
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly IFileCabinetService service;

        /// <summary>Initializes a new instance of the <see cref="RecordMeter" /> class.</summary>
        /// <param name="service">The service.</param>
        public RecordMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>Gets the validator.</summary>
        /// <returns>Returns the validator.</returns>
        public IRecordValidator GetValidator()
        {
            this.stopwatch.Start();
            IRecordValidator returnValidator = this.service.GetValidator();
            this.stopwatch.Stop();
            Console.WriteLine($"GetValidator method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValidator;
        }

        /// <summary>Creates the record.</summary>
        /// <param name="newRecordData">The new record data.</param>
        /// <returns>Returns the id of a created record.</returns>
        public int CreateRecord(RecordData newRecordData)
        {
            this.stopwatch.Start();
            int returnValue = this.service.CreateRecord(newRecordData);
            this.stopwatch.Stop();
            Console.WriteLine($"CreateRecord method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValue;
        }

        /// <summary>Edits the record.</summary>
        /// <param name="newRecordData">The new record data.</param>
        public void EditRecord(RecordData newRecordData)
        {
            this.stopwatch.Start();
            this.service.EditRecord(newRecordData);
            this.stopwatch.Stop();
            Console.WriteLine($"EditRecord method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
        }

        /// <summary>Deletes the record with the specified id.</summary>
        /// <param name="id">The identifier.</param>
        public void DeleteRecord(int id)
        {
            this.stopwatch.Start();
            this.service.DeleteRecord(id);
            this.stopwatch.Stop();
            Console.WriteLine($"DeleteRecord method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
        }

        /// <summary>Finds the records by firstname.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Returns records with the specified first name.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> returnCollection = this.service.FindByFirstName(firstName);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnCollection;
        }

        /// <summary>Finds the records by lastname.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Returns records with the specified last name.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> returnCollection = this.service.FindByLastName(lastName);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByLastName method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnCollection;
        }

        /// <summary>Finds the records by date of birth.</summary>
        /// <param name="dateTime">The date of birth.</param>
        /// <returns>Returns records with the specified date of birth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> returnCollection = this.service.FindByDateOfBirth(dateTime);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByDateOfBirth method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnCollection;
        }

        /// <summary>Gets the records.</summary>
        /// <returns>Returns the collection of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stopwatch.Start();
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.GetRecords();
            this.stopwatch.Stop();
            Console.WriteLine($"GetRecords method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnCollection;
        }

        /// <summary>Makes the snapshot.</summary>
        /// <returns>Returns the snapshot of current FileCabinet.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.stopwatch.Start();
            FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();
            this.stopwatch.Stop();
            Console.WriteLine($"MakeSnapshot method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return snapshot;
        }

        /// <summary>Restores the FileCabinet with specified snapshot.</summary>
        /// <param name="snapshot">The snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopwatch.Start();
            this.service.Restore(snapshot);
            this.stopwatch.Stop();
            Console.WriteLine($"Restore method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
        }

        /// <summary>Purges this instance.</summary>
        /// <returns>Amount of purged elements.</returns>
        public int Purge()
        {
            this.stopwatch.Start();
            int returnValue = this.service.Purge();
            this.stopwatch.Stop();
            Console.WriteLine($"Purge method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValue;
        }

        /// <summary>Gets the stat.</summary>
        /// <returns>Returns the count of records.</returns>
        public int GetStat()
        {
            this.stopwatch.Start();
            int returnValue = this.service.GetStat();
            this.stopwatch.Stop();
            Console.WriteLine($"GetStat method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValue;
        }

        /// <summary>Gets the removed stat.</summary>
        /// <returns>Returns the count of removed records.</returns>
        public int GetRemovedStat()
        {
            this.stopwatch.Start();
            int returnValue = this.service.GetRemovedStat();
            this.stopwatch.Stop();
            Console.WriteLine($"GetRemovedStat method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValue;
        }

        /// <summary>Gets the select dictionary.</summary>
        /// <returns>Returns the select dictionary.</returns>
        public Dictionary<string, string> GetSelectDictionary()
        {
            this.stopwatch.Start();
            Dictionary<string, string> dic = this.service.GetSelectDictionary();
            this.stopwatch.Stop();
            Console.WriteLine($"GetSelectDictionary method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            return dic;
        }
    }
}
