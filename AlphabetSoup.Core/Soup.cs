using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Maintains the data of the alphabet soup
    /// </summary>
    class Soup {

        /// <summary>
        /// The soup itself
        /// </summary>
        public int[,] Matrix { get; set; }

        /// <summary>
        /// The list of used words to build the alphabet soup
        /// </summary>
        public Dictionary<string, Word> UsedWords { get; set; }


        /// <summary>
        /// Generate a new alphabet soup following the directives set in the <see cref="Options"/>
        /// </summary>
        /// <param name="options"><see cref="Options"/> settings</param>
        /// <returns>Initialized <see cref="Soup"/> with random data</returns>
        public static Soup Build(Options options) {
            var generator = new SoupGenerator(options);
            return generator.Init()
                .Create();
        }

    }
}
