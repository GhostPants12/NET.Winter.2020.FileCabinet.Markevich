using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp.FileCabinetService
{
    public class RecordMeter : IFileCabinetService
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private IFileCabinetService service;

        public RecordMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        public IRecordValidator GetValidator()
        {
            this.stopwatch.Start();
            IRecordValidator returnValidator = this.service.GetValidator();
            this.stopwatch.Stop();
            Console.WriteLine($"GetValidator method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValidator;
        }

        public int CreateRecord(RecordData newRecordData)
        {
            this.stopwatch.Start();
            int returnValue = this.service.CreateRecord(newRecordData);
            this.stopwatch.Stop();
            Console.WriteLine($"CreateRecord method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValue;
        }

        public void EditRecord(RecordData newRecordData)
        {
            this.stopwatch.Start();
            this.service.EditRecord(newRecordData);
            this.stopwatch.Stop();
            Console.WriteLine($"EditRecord method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
        }

        public void DeleteRecord(int id)
        {
            this.stopwatch.Start();
            this.service.DeleteRecord(id);
            this.stopwatch.Stop();
            Console.WriteLine($"DeleteRecord method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch.Start();
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.FindByFirstName(firstName);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnCollection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopwatch.Start();
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.FindByLastName(lastName);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByLastName method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnCollection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            this.stopwatch.Start();
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.FindByDateOfBirth(dateTime);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByDateOfBirth method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnCollection;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stopwatch.Start();
            ReadOnlyCollection<FileCabinetRecord> returnCollection = this.service.GetRecords();
            this.stopwatch.Stop();
            Console.WriteLine($"GetRecords method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnCollection;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.stopwatch.Start();
            FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();
            this.stopwatch.Stop();
            Console.WriteLine($"MakeSnapshot method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return snapshot;
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopwatch.Start();
            this.service.Restore(snapshot);
            this.stopwatch.Stop();
            Console.WriteLine($"Restore method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
        }

        public int Purge()
        {
            this.stopwatch.Start();
            int returnValue = this.service.Purge();
            this.stopwatch.Stop();
            Console.WriteLine($"Purge method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValue;
        }

        public int GetStat()
        {
            this.stopwatch.Start();
            int returnValue = this.service.GetStat();
            this.stopwatch.Stop();
            Console.WriteLine($"GetStat method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValue;
        }

        public int GetRemovedStat()
        {
            this.stopwatch.Start();
            int returnValue = this.service.GetRemovedStat();
            this.stopwatch.Stop();
            Console.WriteLine($"GetRemovedStat method execution duration is {this.stopwatch.Elapsed.Ticks} ticks.");
            this.stopwatch.Reset();
            return returnValue;
        }
    }
}
