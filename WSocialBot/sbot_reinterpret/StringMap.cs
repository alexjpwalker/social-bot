using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSocialBot.sbot_reinterpret
{
    /// <summary>
    /// Represents a mapping from a set of original strings to a single, reinterpreted string.
    /// </summary>
    class StringMap
    {
        public string[] Originals { get; set; }
        public string Result { get; set; }
        public int Length { get; private set; }

        public StringMap(string[] originals, string result)
        {
            this.Originals = new string[8];
            this.Result = result;
            this.Length = 0;
            this.Add(originals);
        }
        public StringMap(string result)
        {
            this.Originals = new string[8];
            this.Length = 0;
            this.Result = result;
        }
        public StringMap()
        {
            this.Originals = new string[8];
            this.Length = 0;
            this.Result = String.Empty;
        }

        /// <summary>
        /// Add a new original word to the StringMap.
        /// </summary>
        /// <param name="newOriginal">The new word to be added.</param>
        public void Add(string newOriginal)
        {
            if (Originals.Length == this.Length) // Expand array if capacity is reached
            {
                string[] newList = new string[this.Length * 2];
                for (int i = 0; i < this.Length; i++)
                    newList[i] = Originals[i];
                Originals = newList;
            }
            Originals[Length] = newOriginal;
            Length++;
        }

        /// <summary>
        /// Add a list of new original words to the StringMap.
        /// </summary>
        /// <param name="originals">The new words to be added.</param>
        public void Add(string[] originals)
        {
            for (int i = 0; i < originals.Length; i++)
                this.Add(originals[i]);
        }
    }
}
