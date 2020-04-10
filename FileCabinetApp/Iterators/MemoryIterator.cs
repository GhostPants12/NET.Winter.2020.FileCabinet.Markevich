using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.FileCabinetService
{
    public class MemoryIterator : IEnumerator<FileCabinetRecord>
    {
        private List<FileCabinetRecord> records;

        private int currentPosition = -1;

        public MemoryIterator(List<FileCabinetRecord> records)
        {
            this.records = records;
        }

        public FileCabinetRecord Current => this.records[currentPosition];

        object IEnumerator.Current => this.Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            this.currentPosition++;
            return this.currentPosition <= this.records.Count;
        }

        public void Reset()
        {
            this.currentPosition = 0;
        }
    }
}
