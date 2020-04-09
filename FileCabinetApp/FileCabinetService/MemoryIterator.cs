using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.FileCabinetService
{
    public class MemoryIterator : IRecordIterator
    {
        private List<FileCabinetRecord> records;

        private int currentPosition = 0;

        public MemoryIterator(List<FileCabinetRecord> records)
        {
            this.records = records;
        }

        public FileCabinetRecord GetNext()
        {
            if (this.HasMore())
            {
                this.currentPosition++;
            }

            return this.records[currentPosition];
        }

        public bool HasMore()
        {
            return this.currentPosition < this.records.Count - 1;
        }
    }
}
