using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.RecordPrinter
{
    public class DefaultRecordPrinter : IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
            }
        }
    }
}
