using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Class for reading csv file.</summary>
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader streamReader;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordCsvReader" /> class.</summary>
        /// <param name="sr">The sr.</param>
        public FileCabinetRecordCsvReader(StreamReader sr)
        {
            this.streamReader = sr;
        }

        /// <summary>Reads all file.</summary>
        /// <returns>Returns the list of FileCabinetRecord that file contains.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> resultList = new List<FileCabinetRecord>();
            this.streamReader.ReadLine();
            string strToImport = this.streamReader.ReadLine();
            if (strToImport != null)
            {
                int possibleErrorId = -1;
                while (!string.IsNullOrEmpty(strToImport))
                {
                    try
                    {
                        var strArr = strToImport.Split(", ");
                        possibleErrorId = int.Parse(strArr[0], provider: CultureInfo.InvariantCulture);
                        resultList.Add(new FileCabinetRecord()
                        {
                            Id = possibleErrorId,
                            FirstName = strArr[1],
                            LastName = strArr[2],
                            DateOfBirth = DateTime.ParseExact(strArr[3], "MM/dd/yyyy", CultureInfo.InvariantCulture),
                            Code = short.Parse(strArr[4], CultureInfo.InvariantCulture),
                            Letter = char.Parse(strArr[5]),
                            Balance = decimal.Parse(strArr[6], CultureInfo.InvariantCulture),
                        });
                        strToImport = this.streamReader.ReadLine();
                    }
                    #pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex)
                    #pragma warning restore CA1031 // Do not catch general exception types
                    {
                        if (possibleErrorId >= 0)
                        {
                            Console.WriteLine($"Id: {possibleErrorId}");
                        }

                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return resultList;
        }
    }
}
