using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetGenerator
{
    public class CSVWriter
    {
        private TextWriter writer;

        public CSVWriter(TextWriter writer)
        {
            this.writer = writer;
            this.writer.Write("Id, First Name, Last Name, Date of Birth, Code, Letter, Balance");
        }


        public void Generate(List<FileCabinetApp.FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                this.writer.WriteLine();
                this.writer.Write(record.Id + ", " + record.FirstName + ", " + record.LastName + ", " + record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + ", " + record.Code + ", " + record.Letter + ", " + record.Balance);
            }
        }
    }
}
