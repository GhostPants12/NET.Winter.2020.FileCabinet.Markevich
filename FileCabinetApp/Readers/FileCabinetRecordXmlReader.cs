using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>Class for reading xml file and converting it to the FileCabinetRecord collection.</summary>
    public class FileCabinetRecordXmlReader
    {
        private FileStream fileStream;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordXmlReader" /> class.</summary>
        /// <param name="fs">The fs.</param>
        public FileCabinetRecordXmlReader(FileStream fs)
        {
            this.fileStream = fs;
        }

        /// <summary>Reads all the file and converts it to the FileCabinetRecord collection.</summary>
        /// <returns>Returns the collection of FileCabinetRecord.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> returnList = new List<FileCabinetRecord>();
#pragma warning disable CA5369 // Use XmlReader For Deserialize
            foreach (var element in ((XmlContainer)new XmlSerializer(typeof(XmlContainer)).Deserialize(this.fileStream)).Records)
#pragma warning restore CA5369 // Use XmlReader For Deserialize
            {
                returnList.Add(element);
            }

            return returnList;
        }
    }
}
