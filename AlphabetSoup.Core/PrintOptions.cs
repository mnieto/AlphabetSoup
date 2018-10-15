using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Select what and how to print the alphabet soup
    /// </summary>
    public class PrintOptions {

        ///<summary>Prints the rectangle with the alphabet soup</summary>
        public bool PrintAlphabetSoup { get; set; } = true;

        ///<summary>Prints the words list</summary>
        public bool PrintWords { get; set; } = true;

        ///<summary>Remarks the solution inside the alphabet soup</summary>
        public bool PrintSolution { get; set; } = false;

        /// <summary>
        /// Gets or set the number of columns used to output the list of used words. 
        /// Default is <c>null</c>. In that case it attempts to use as many columns as possible
        /// Value of 1 indicates one word per line
        /// </summary>
        public int? WordColumns { get; set; } = null;
    }
}
