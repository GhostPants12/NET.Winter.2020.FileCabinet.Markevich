using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp
{
    /// <summary>Interface for the file cabinet services.</summary>
    public interface IFileCabinetService
    {
        /// <summary>Gets the validator.</summary>
        /// <returns>Returns the validator.</returns>
        RecordValidator.IRecordValidator GetValidator();

        /// <summary>Creates the record.</summary>
        /// <param name="newRecordData">The new record data.</param>
        /// <returns>Returns the id of a created record.</returns>
        int CreateRecord(RecordData newRecordData);

        /// <summary>Edits the record.</summary>
        /// <param name="newRecordData">The new record data.</param>
        void EditRecord(RecordData newRecordData);

        /// <summary>Deletes the record with the specified id.</summary>
        /// <param name="id">The identifier.</param>
        void DeleteRecord(int id);

        /// <summary>Finds the records by firstname.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Returns records with the specified first name.</returns>
        IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>Finds the records by lastname.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Returns records with the specified last name.</returns>
        IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>Finds the records by date of birth.</summary>
        /// <param name="dateTime">The date of birth.</param>
        /// <returns>Returns records with the specified date of birth.</returns>
        IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime);

        /// <summary>Gets the records.</summary>
        /// <returns>Returns the collection of records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>Makes the snapshot.</summary>
        /// <returns>Returns the snapshot of current FileCabinet.</returns>
        FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>Restores the FileCabinet with specified snapshot.</summary>
        /// <param name="snapshot">The snapshot.</param>
        void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>Purges this instance.</summary>
        /// <returns>Amount of purged elements.</returns>
        int Purge();

        /// <summary>Gets the stat.</summary>
        /// <returns>Returns the count of records.</returns>
        int GetStat();

        /// <summary>Gets the removed stat.</summary>
        /// <returns>Returns the count of removed records.</returns>
        int GetRemovedStat();

        /// <summary>Gets the select dictionary.</summary>
        /// <returns>Returns the select dictionary.</returns>
        Dictionary<string, string> GetSelectDictionary();
    }
}
