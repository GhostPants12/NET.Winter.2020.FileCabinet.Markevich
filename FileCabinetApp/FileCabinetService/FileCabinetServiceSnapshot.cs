using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Writers;

namespace FileCabinetApp
{
    /// <summary>Class-container for FileCabinet exporting.</summary>
    public class FileCabinetServiceSnapshot
    {
        /// <summary>The records.</summary>
        private FileCabinetRecord[] records;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetServiceSnapshot" /> class.</summary>
        /// <param name="list">The list of records.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            if (list != null)
            {
                this.records = list.ToArray();
            }
        }

        /// <summary>Gets the records.</summary>
        /// <value>The records.</value>
        public ReadOnlyCollection<FileCabinetRecord> Records => Array.AsReadOnly(this.records);

        /// <summary>Saves snapshot to CSV format.</summary>
        /// <param name="sr">The stream for a file.</param>
        public void SaveToCsv(StreamWriter sr)
        {
            FileCabinetRecordCsvWriter writer = new FileCabinetRecordCsvWriter(sr);
            for (int i = 0; i < this.records.Length; i++)
            {
                writer.Write(this.records[i]);
            }
        }

        /// <summary>Loads the snapshot from CSV file.</summary>
        /// <param name="sr">The stream for a file.</param>
        public void LoadFromCsv(StreamReader sr)
        {
            int overwrittenElements = 0;
            int lastElementIndex = this.records.Length - 1;
            IList<FileCabinetRecord> list = new FileCabinetRecordCsvReader(sr).ReadAll();
            Array.Resize(ref this.records, this.records.Length + list.Count);
            foreach (var element in list)
            {
                if (lastElementIndex < 0)
                {
                    this.records[lastElementIndex + 1] = element;
                    lastElementIndex++;
                    continue;
                }

                for (int i = 0; i <= lastElementIndex; i++)
                {
                    if (this.records[i].Id == element.Id)
                    {
                        this.records[i] = element;
                        overwrittenElements++;
                        break;
                    }

                    if (i == lastElementIndex)
                    {
                        this.records[lastElementIndex + 1] = element;
                        lastElementIndex++;
                        break;
                    }
                }
            }

            Array.Resize(ref this.records, this.records.Length - overwrittenElements);
        }

        /// <summary>Saves snapshot to XML file.</summary>
        /// <param name="sr">The stream for a file.</param>
        public void SaveToXml(StreamWriter sr)
        {
            FileCabinetRecordXmlWriter writer = new FileCabinetRecordXmlWriter(sr);
            for (int i = 0; i < this.records.Length; i++)
            {
                writer.Write(this.records[i]);
            }

            writer.EndWriting();
        }

        /// <summary>Loads snapshot from XML file.</summary>
        /// <param name="fs">The stream for a file.</param>
        public void LoadFromXml(FileStream fs)
        {
            int overwrittenElements = 0;
            int lastElementIndex = this.records.Length - 1;
            IList<FileCabinetRecord> list = new FileCabinetRecordXmlReader(fs).ReadAll();
            Array.Resize(ref this.records, this.records.Length + list.Count);
            foreach (var element in list)
            {
                if (lastElementIndex < 0)
                {
                    this.records[lastElementIndex + 1] = element;
                    lastElementIndex++;
                    continue;
                }

                for (int i = 0; i <= lastElementIndex; i++)
                {
                    if (this.records[i].Id == element.Id)
                    {
                        this.records[i] = element;
                        overwrittenElements++;
                        break;
                    }

                    if (i == lastElementIndex)
                    {
                        this.records[lastElementIndex + 1] = element;
                        lastElementIndex++;
                        break;
                    }
                }
            }

            Array.Resize(ref this.records, this.records.Length - overwrittenElements);
        }
    }
}
