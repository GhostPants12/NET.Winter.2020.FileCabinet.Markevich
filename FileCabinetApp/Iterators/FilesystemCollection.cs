using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Iterators
{
    /// <summary>Collection of FileCabinetRecord for the Filesystem FileCabinet.</summary>
    public class FilesystemCollection : IEnumerable<FileCabinetRecord>
    {
        private readonly List<long> indexList;
        private readonly FileStream fileStream;

        /// <summary>Initializes a new instance of the <see cref="FilesystemCollection" /> class.</summary>
        /// <param name="fs">The fs.</param>
        /// <param name="list">The list.</param>
        public FilesystemCollection(FileStream fs, List<long> list)
        {
            this.fileStream = fs;
            this.indexList = list;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new FilesystemIterator(this.fileStream, this.indexList);
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="System.Collections.IEnumerator">IEnumerator</see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
