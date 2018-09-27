using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Word used in alphabet soup. It describes the word and location inside the soup
    /// </summary>
    public class Word {

        /// <summary>
        /// X origin coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y origin coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Direction to write to
        /// </summary>
        public Directions Direction { get; set; }

        /// <summary>
        /// Word itself
        /// </summary>
        public string Name { get; set; }
    }
}
