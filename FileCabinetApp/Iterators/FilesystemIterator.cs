using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.FileCabinetService
{
    /// <summary>Iterator for the Filesystem FileCabinetRecord collection.</summary>
    public sealed class FilesystemIterator : IEnumerator<FileCabinetRecord>
    {
        private readonly List<long> indexList;
        private readonly FileStream fileStream;
        private int index = -1;

        /// <summary>Initializes a new instance of the <see cref="FilesystemIterator" /> class.</summary>
        /// <param name="fs">The fs.</param>
        /// <param name="list">The list.</param>
        public FilesystemIterator(FileStream fs, List<long> list)
        {
            this.fileStream = fs;
            this.indexList = list;
        }

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <value>The element in the collection at the current position of the enumerator.</value>
        public FileCabinetRecord Current
        {
            get
            {
                this.fileStream.Position = this.indexList[this.index];
                int year, month, day;
                int[] decimalArray = new int[4];
                byte[] bytes = new byte[120];
                FileCabinetRecord recordToReturn = new FileCabinetRecord();
                this.fileStream.Read(bytes, 0, 2);
                this.fileStream.Read(bytes, 0, 4);
                recordToReturn.Id = BitConverter.ToInt32(bytes);
                this.fileStream.Read(bytes, 0, 120);
                recordToReturn.FirstName = Encoding.Default.GetString(bytes, 0, 120)
                    .Replace("\0", string.Empty, StringComparison.InvariantCulture);
                this.fileStream.Read(bytes, 0, 120);
                recordToReturn.LastName = Encoding.Default.GetString(bytes, 0, 120)
                    .Replace("\0", string.Empty, StringComparison.InvariantCulture);
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
        }

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <value>The element in the collection at the current position of the enumerator.</value>
        object IEnumerator.Current => this.Current;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///   <span class="keyword">
        ///     <span class="languageSpecificText">
        ///       <span class="cs">true</span>
        ///       <span class="vb">True</span>
        ///       <span class="cpp">true</span>
        ///     </span>
        ///   </span>
        ///   <span class="nu">
        ///     <span class="keyword">true</span> (<span class="keyword">True</span> in Visual Basic)</span> if the enumerator was successfully advanced to the next element; <span class="keyword"><span class="languageSpecificText"><span class="cs">false</span><span class="vb">False</span><span class="cpp">false</span></span></span><span class="nu"><span class="keyword">false</span> (<span class="keyword">False</span> in Visual Basic)</span> if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            this.index++;
            return this.index < this.indexList.Count;
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        public void Reset()
        {
            this.index = 0;
        }
    }
}
