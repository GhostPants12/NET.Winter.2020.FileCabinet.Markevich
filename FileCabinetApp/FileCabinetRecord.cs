using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>Class for the record in the file cabinet.</summary>
    [Serializable]
    [XmlRoot("FileCabinetRecord")]
    public class FileCabinetRecord
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        /// <value>The first name.</value>
        [XmlElement("firstname")]
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        /// <value>The last name.</value>
        [XmlElement("lastname")]
        public string LastName { get; set; }

        /// <summary>Gets or sets the date of birth.</summary>
        /// <value>The date of birth.</value>
        [XmlIgnore]
        public DateTime DateOfBirth { get; set; }

        [XmlElement("dateOfBirth")]
        public string Date
        {
            get => this.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            set => this.DateOfBirth = DateTime.Parse(value, CultureInfo.CurrentCulture);
        }

        /// <summary>Gets or sets the code.</summary>
        /// <value>The code.</value>
        [XmlElement("code")]
        public short Code { get; set; }

        /// <summary>Gets or sets the letter.</summary>
        /// <value>The letter.</value>
        [XmlIgnore]
        public char Letter { get; set; }

        [XmlElement("letter"), Browsable(false)]
        public string TestPropertyString
        {
            get { return this.Letter.ToString(CultureInfo.InvariantCulture); }
            set { this.Letter = value.Single(); }
        }

        /// <summary>Gets or sets the balance.</summary>
        /// <value>The balance.</value>
        [XmlElement("balance")]
        public decimal Balance { get; set; }
    }
}
