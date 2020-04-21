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

        IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime);

        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        FileCabinetServiceSnapshot MakeSnapshot();

        void Restore(FileCabinetServiceSnapshot snapshot);

        int Purge();

        int GetStat();

        int GetRemovedStat();

        Dictionary<string, string> GetSelectDictionary();
    }
}
