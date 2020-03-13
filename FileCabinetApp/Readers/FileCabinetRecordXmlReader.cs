using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlReader
    {
        private FileStream fileStream;

        public FileCabinetRecordXmlReader(FileStream fs)
        {
            this.fileStream = fs;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> returnList = new List<FileCabinetRecord>();
            foreach (var element in ((XMLContainer)new XmlSerializer(typeof(XMLContainer)).Deserialize(fileStream)).Records)
            {
                returnList.Add(element);
            }

            return returnList;
        }
    }
}
