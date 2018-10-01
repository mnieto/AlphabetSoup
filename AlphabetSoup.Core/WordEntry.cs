using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Word used in alphabet soup. It describes the word and location inside the soup
    /// </summary>
    public class WordEntry {

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public (int X, int Y) AbsoluteOrigin() {
            if (Direction.IsReverse()) {
                switch (Direction) {
                    case Directions.S:
                        return (X, Y - Name.Length + 1);
                    case Directions.NW:
                        return (X, Y - Name.Length + 1);
                    case Directions.SW:
                        return (X - Name.Length + 1, Y - Name.Length + 1);
                    case Directions.W:
                        return (X - Name.Length+ 1, Y);
                }
            }
            return (X, Y);
        }
    }
}
