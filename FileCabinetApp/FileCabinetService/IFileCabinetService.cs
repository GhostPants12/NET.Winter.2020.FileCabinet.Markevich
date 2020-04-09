using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        RecordValidator.IRecordValidator GetValidator();

        int CreateRecord(RecordData newRecordData);

        void EditRecord(RecordData newRecordData);

        void DeleteRecord(int id);

        IRecordIterator FindByFirstName(string firstName);

        IRecordIterator FindByLastName(string lastName);

        IRecordIterator FindByDateOfBirth(DateTime dateTime);

        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        FileCabinetServiceSnapshot MakeSnapshot();

        void Restore(FileCabinetServiceSnapshot snapshot);

        int Purge();

        int GetStat();

        int GetRemovedStat();
    }
}
