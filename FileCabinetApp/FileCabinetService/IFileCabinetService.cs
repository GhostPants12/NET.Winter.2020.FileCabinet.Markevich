using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        IRecordValidator.IRecordValidator GetValidator();

        int CreateRecord(RecordData newRecordData);

        void EditRecord(RecordData newRecordData);

        void DeleteRecord(int id);

        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime);

        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        FileCabinetServiceSnapshot MakeSnapshot();

        void Restore(FileCabinetServiceSnapshot snapshot);

        int GetStat();
    }
}
