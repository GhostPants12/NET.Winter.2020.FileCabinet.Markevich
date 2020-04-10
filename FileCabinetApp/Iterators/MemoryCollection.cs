using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Iterators
{
    public class MemoryCollection : IEnumerable<FileCabinetRecord>
    {
        private List<FileCabinetRecord> records;

        public MemoryCollection(List<FileCabinetRecord> records)
        {
            this.records = records;
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new MemoryIterator(this.records);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
