using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        int CreateRecord(RecordData newRecordData);

        void EditRecord(RecordData newRecordData);

        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime);

        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        int GetStat();
    }
}
