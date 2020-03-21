using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordPrinter
{
    public interface IRecordPrinter
    {
        void Print(IEnumerable<FileCabinetRecord> records);
    }
}
