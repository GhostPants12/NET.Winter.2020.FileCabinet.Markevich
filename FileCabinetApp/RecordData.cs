using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Class-container for the fields of FileCabinetRecord.</summary>
    public class RecordData
    {
        /// <summary>Initializes a new instance of the <see cref="RecordData"/> class.</summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="code">The code.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        public RecordData(string firstName, string lastName, short code, char letter, decimal balance, DateTime dateOfBirth)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Code = code;
            this.Letter = letter;
            this.Balance = balance;
            this.DateOfBirth = dateOfBirth;
        }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets the first name.</summary>
        /// <value>The first name.</value>
        public string FirstName { get; }

        /// <summary>Gets the last name.</summary>
        /// <value>The last name.</value>
        public string LastName { get; }

        /// <summary>Gets the code.</summary>
        /// <value>The code.</value>
        public short Code { get; }

        /// <summary>Gets the letter.</summary>
        /// <value>The letter.</value>
        public char Letter { get; }

        /// <summary>Gets the balance.</summary>
        /// <value>The balance.</value>
        public decimal Balance { get; }

        /// <summary>Gets the date of birth.</summary>
        /// <value>The date of birth.</value>
        public DateTime DateOfBirth { get; }
    }
}
