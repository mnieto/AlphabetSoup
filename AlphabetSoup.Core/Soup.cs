using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Maintains the data of the alphabet soup
    /// </summary>
    public class Soup {

        /// <summary>
        /// The soup itself
        /// </summary>
        public char[,] Matrix { get; set; }

        /// <summary>
        /// Controls what cells in <see cref="Matrix"/> are used
        /// </summary>
        public bool[,] ShadowMatrix { get; set; }

        /// <summary>
        /// The list of used words to build the alphabet soup
        /// </summary>
        public Dictionary<string, WordEntry> UsedWords { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Soup() {
            UsedWords = new Dictionary<string, WordEntry>();
        }

        /// <summary>
        /// Generate a new alphabet soup following the directives set in the <see cref="Options"/>
        /// </summary>
        public static Soup Build(IServiceProvider services) {
        var generator = services.GetService<ISoupGenerator>();
            return generator.Init()
                .Create();
        }

    }
}
