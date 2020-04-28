using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp;
using FileCabinetGenerator.Generator;

namespace FileCabinetGenerator
{
    class XMLWriter
    {
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(XmlContainer));
        private FileStream fs;

        public XMLWriter(FileStream fs)
        {
            this.fs = fs;
        }

        public void Generate(XmlContainer container)
        {
            serializer.Serialize(fs, container);
        }
    }
}
