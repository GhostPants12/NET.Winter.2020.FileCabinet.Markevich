using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Class for the record in the file cabinet.</summary>
    public class FileCabinetRecord
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>Gets or sets the code.</summary>
        /// <value>The code.</value>
        public short Code { get; set; }

        /// <summary>Gets or sets the letter.</summary>
        /// <value>The letter.</value>
        public char Letter { get; set; }

        /// <summary>Gets or sets the balance.</summary>
        /// <value>The balance.</value>
        public decimal Balance { get; set; }

        /// <summary>Gets or sets the date of birth.</summary>
        /// <value>The date of birth.</value>
        public DateTime DateOfBirth { get; set; }
    }
}
