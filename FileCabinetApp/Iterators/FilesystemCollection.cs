using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Iterators
{
    public class FilesystemCollection : IEnumerable<FileCabinetRecord>
    {
        private readonly List<long> indexList;
        private readonly FileStream fileStream;

        public FilesystemCollection(FileStream fs, List<long> list)
        {
            this.fileStream = fs;
            this.indexList = list;
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new FilesystemIterator(fileStream, indexList);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
