using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Iterators
{
    /// <summary>Container for the records in memory file cabinet service.</summary>
    public class MemoryCollection : IEnumerable<FileCabinetRecord>
    {
        private List<FileCabinetRecord> records;

        /// <summary>Initializes a new instance of the <see cref="MemoryCollection" /> class.</summary>
        /// <param name="records">The records.</param>
        public MemoryCollection(List<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new MemoryIterator(this.records);
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="System.Collections.IEnumerator">IEnumerator</see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
