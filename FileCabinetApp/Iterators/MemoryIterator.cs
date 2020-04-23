using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.FileCabinetService
{
    /// <summary>Iterator for MemoryCollection class.</summary>
    public sealed class MemoryIterator : IEnumerator<FileCabinetRecord>
    {
        private List<FileCabinetRecord> records;

        private int currentPosition = -1;

        /// <summary>Initializes a new instance of the <see cref="MemoryIterator" /> class.</summary>
        /// <param name="records">The records.</param>
        public MemoryIterator(List<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <value>The element in the collection at the current position of the enumerator.</value>
        public FileCabinetRecord Current => this.records[this.currentPosition];

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
            this.currentPosition++;
            return this.currentPosition <= this.records.Count;
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        public void Reset()
        {
            this.currentPosition = 0;
        }
    }
}
