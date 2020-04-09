using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.FileCabinetService
{
    public class FilesystemIterator : IRecordIterator
    {
        private FileStream fileStream;
        private int index = 0;
        private List<long> indexList;

        public FilesystemIterator(FileStream fs, List<long> list)
        {
            this.fileStream = fs;
            this.indexList = list;
        }

        public FileCabinetRecord GetNext()
        {
            this.fileStream.Position = this.indexList[this.index];

            if (this.HasMore())
            {
                this.index++;
            }

            int year, month, day;
            int[] decimalArray = new int[4];
            byte[] bytes = new byte[120];
            FileCabinetRecord recordToReturn = new FileCabinetRecord();
            this.fileStream.Read(bytes, 0, 2);
            this.fileStream.Read(bytes, 0, 4);
            recordToReturn.Id = BitConverter.ToInt32(bytes);
            this.fileStream.Read(bytes, 0, 120);
            recordToReturn.FirstName = Encoding.Default.GetString(bytes, 0, 120).Replace("\0", string.Empty, StringComparison.InvariantCulture);
            this.fileStream.Read(bytes, 0, 120);
            recordToReturn.LastName = Encoding.Default.GetString(bytes, 0, 120).Replace("\0", string.Empty, StringComparison.InvariantCulture);
            this.fileStream.Read(bytes, 0, 4);
            year = BitConverter.ToInt32(bytes);
            this.fileStream.Read(bytes, 0, 4);
            month = BitConverter.ToInt32(bytes);
            this.fileStream.Read(bytes, 0, 4);
            day = BitConverter.ToInt32(bytes);
            recordToReturn.DateOfBirth = new DateTime(year, month, day);
            this.fileStream.Read(bytes, 0, 2);
            recordToReturn.Code = BitConverter.ToInt16(bytes);
            this.fileStream.Read(bytes, 0, 2);
            recordToReturn.Letter = BitConverter.ToChar(bytes);
            for (int i = 0; i < decimalArray.Length; i++)
            {
                this.fileStream.Read(bytes, 0, 4);
                decimalArray[i] = BitConverter.ToInt32(bytes);
            }

            recordToReturn.Balance = new decimal(decimalArray);
            return recordToReturn;
        }

        public bool HasMore()
        {
            return this.index < this.indexList.Count - 1;
        }
    }
}
