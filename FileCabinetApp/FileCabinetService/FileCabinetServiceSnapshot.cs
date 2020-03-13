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
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.records = list.ToArray();
        }

        public ReadOnlyCollection<FileCabinetRecord> Records
        {
            get => Array.AsReadOnly(this.records);
        }

        public void SaveToCsv(StreamWriter sr)
        {
            FileCabinetRecordCsvWriter writer = new FileCabinetRecordCsvWriter(sr);
            for (int i = 0; i < this.records.Length; i++)
            {
                writer.Write(this.records[i]);
            }
        }

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

        public void SaveToXml(StreamWriter sr)
        {
            FileCabinetRecordXmlWriter writer = new FileCabinetRecordXmlWriter(sr);
            for (int i = 0; i < this.records.Length; i++)
            {
                writer.Write(this.records[i]);
            }

            writer.EndWriting();
        }
    }
}
