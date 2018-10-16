using System;
using System.Collections.Generic;

namespace AlphabetSoup.Core {

    public interface IPrinter {

        /// <summary>
        /// What settings have into account to print the alphabet soup
        /// </summary>
        PrintOptions Options { get; set; }

        /// <summary>
        /// Print the alphabet soup following the settings of <see cref="PrintOptions"/>
        /// </summary>
        /// <param name="soup"><see cref="Soup"/> to be printed</param>
        /// <remarks>
        /// Usually it calls in turn to <see cref="PrintSoup(Soup)"/> and <see cref="PrintWords(IEnumerable{string})"/> Depending on the options set in <see cref="PrintOptions"/>
        /// </remarks>
        void Print(Soup soup);

        /// <summary>
        /// Outputs the soup. It will take into account the set up options like <see cref="PrintOptions.PrintSolution"/>
        /// </summary>
        /// <param name="soup"><see cref="Soup"/> to be printed out</param>
        void PrintSoup(Soup soup);

        /// <summary>
        /// Outputs the list of words used in the soup. It will take into account the set up options in <see cref="PrintOptions"/>
        /// </summary>
        /// <param name="words">List of words to be output</param>
        void PrintWords(IEnumerable<string> words);
    }
}
