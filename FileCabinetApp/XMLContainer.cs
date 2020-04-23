using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>XML Container for the array of FileCabinetRecord.</summary>
    [Serializable]
    public class XmlContainer
    {
        /// <summary>Initializes a new instance of the <see cref="XmlContainer" /> class.</summary>
        public XmlContainer()
        {
        }

        /// <summary>Gets or sets the records.</summary>
        /// <value>The records.</value>
        [XmlArray("records")]
        [XmlArrayItem(typeof(FileCabinetRecord), ElementName = "record")]
#pragma warning disable CA2227 // Collection properties should be read only
#pragma warning disable CA2235 // Mark all non-serializable fields
        public List<FileCabinetRecord> Records { get; set; }
#pragma warning restore CA2235 // Mark all non-serializable fields
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
