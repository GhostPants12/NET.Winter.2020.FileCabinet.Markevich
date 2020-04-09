using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.FileCabinetService
{
    public interface IRecordIterator
    {
        FileCabinetRecord GetNext();

        bool HasMore();
    }
}
