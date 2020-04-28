using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp.Writers
{
    /// <summary>Class for writing records to xml file.</summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriterSettings settings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "    ",
        };

        private readonly XmlWriter writer;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter" /> class.</summary>
        /// <param name="writer">The writer.</param>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            this.writer = XmlWriter.Create(writer, this.settings);
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement("records");
        }

        /// <summary>Writes the specified record to the file.</summary>
        /// <param name="record">The record.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteStartElement("record");
            if (record != null)
            {
                this.writer.WriteAttributeString("id", record.Id.ToString(CultureInfo.InvariantCulture));
                this.writer.WriteStartElement("firstname");
                this.writer.WriteString(record.FirstName.ToString(CultureInfo.InvariantCulture));
                this.writer.WriteEndElement();
                this.writer.WriteStartElement("lastname");
                this.writer.WriteString(record.LastName.ToString(CultureInfo.InvariantCulture));
                this.writer.WriteEndElement();
                this.writer.WriteStartElement("dateOfBirth");
                this.writer.WriteString(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                this.writer.WriteEndElement();
                this.writer.WriteStartElement("code");
                this.writer.WriteString(record.Code.ToString(CultureInfo.InvariantCulture));
                this.writer.WriteEndElement();
                this.writer.WriteStartElement("letter");
                this.writer.WriteString(record.Letter.ToString(CultureInfo.InvariantCulture));
                this.writer.WriteEndElement();
                this.writer.WriteStartElement("balance");
                this.writer.WriteString(record.Balance.ToString(CultureInfo.InvariantCulture));
                this.writer.WriteEndElement();
                this.writer.WriteEndElement();
            }
        }

        /// <summary>Ends the writing.</summary>
        public void EndWriting()
        {
            this.writer.WriteEndElement();
            this.writer.WriteEndDocument();
            this.writer.Close();
        }
    }
}
