using System;
using System.Collections.Generic;

namespace AlphabetSoup.Core
{

    /// <summary>
    /// Options used to build the soup
    /// </summary>
    public class Options
    {

        /// <summary>
        /// Culture that will define language information like the list of words, allowed letters,...
        /// </summary>
        public string CultureCode { get; set; }

        /// <summary>
        /// List of words to pick up from, used to build the soup
        /// </summary>
        /// <remarks>
        /// <para>
        /// The size of the list must be equal or greater than <see cref="NumWords"/>
        /// </para>
        /// <para>
        /// If this option is used, the <see cref="LanguageData.LemmataFile"/> file is ignored. If this option is null or empty, the words are picked from the see <cref="LanguageData.LemmataFile"/>.
        /// </para>
        /// </remarks>
        public IEnumerable<string> Words { get; set; }

        /// <summary>
        /// Number of columns and rows (square) of the alphabet soup
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Which directions can use to generate words, by default allowed all left to right directions
        /// </summary>
        public Directions AllowedDirections { get; set; } = Directions.N | Directions.E | Directions.NE | Directions.SE;

        /// <summary>
        /// Number of words generated in the soup
        /// </summary>
        public int NumWords { get; set; }

        /// <summary>
        /// Minimum length for the generated word. If set to 0 any length is allowed
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// Max length for the generated word. If set to 0 any length is allowed
        /// </summary>
        public int MaxLength { get; set; }


    }
}
