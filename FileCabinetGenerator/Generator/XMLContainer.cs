using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FileCabinetApp;

namespace FileCabinetGenerator.Generator
{
    [Serializable]
    public class XMLContainer
    {
        [XmlArray("records"), XmlArrayItem(typeof(FileCabinetRecord), ElementName = "record")]
        public List<FileCabinetRecord> Records { get; set; }

        public XMLContainer()
        {

        }
    }
}
