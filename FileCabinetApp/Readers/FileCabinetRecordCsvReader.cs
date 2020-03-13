using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvReader
    {
        private StreamReader streamReader;

        public FileCabinetRecordCsvReader(StreamReader sr)
        {
            this.streamReader = sr;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> resultList = new List<FileCabinetRecord>();
            this.streamReader.ReadLine();
            string strToImport = this.streamReader.ReadLine();
            string[] strArr = strToImport.Split(", ");
            int possibleErrorId = -1;
            while (!string.IsNullOrEmpty(strToImport))
            {
                try
                {
                    strArr = strToImport.Split(", ");
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
                catch (Exception ex)
                {
                    if (possibleErrorId >= 0)
                    {
                        Console.WriteLine($"Id: {possibleErrorId}");
                    }

                    Console.WriteLine(ex.Message);
                }
            }

            return resultList;
        }
    }
}
