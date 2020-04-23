using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileCabinetApp.Writers
{
    /// <summary>Class for writing records to csv file.</summary>
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter" /> class.</summary>
        /// <param name="writer">The writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
            this.writer?.Write("Id, First Name, Last Name, Date of Birth, Code, Letter, Balance");
        }

        /// <summary>Writes the specified record to the file.</summary>
        /// <param name="record">The record to write.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine();
            if (record != null)
            {
                this.writer.Write(record.Id + ", " + record.FirstName + ", " + record.LastName + ", " +
                                  record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + ", " +
                                  record.Code + ", " + record.Letter + ", " + record.Balance);
            }
        }
    }
}
