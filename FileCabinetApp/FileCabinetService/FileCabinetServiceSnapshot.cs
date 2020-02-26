using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.Writers;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            records = list.ToArray();
        }

        public void SaveToCsv(StreamWriter sr)
        {
            FileCabinetRecordCsvWriter writer = new FileCabinetRecordCsvWriter(sr);
            for (int i = 0; i < records.Length; i++)
            {
                writer.Write(records[i]);
            }
        }
    }
}
