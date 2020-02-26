using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileCabinetApp.Writers
{
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
            this.writer.Write("Id, First Name, Last Name, Date of Birth, Code, Letter, Balance");
        }

        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine();
            this.writer.Write(record.Id + ", " + record.FirstName + ", " + record.LastName + ", " + record.DateOfBirth + ", " + record.Code + ", " + record.Letter + ", " + record.Balance);
        }
    }
}
