using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Select what to print
    /// </summary>
    public enum PrintOptions {

        ///<summary>No print</summary>
        None = 0x0,

        ///<summary>Prints the rectangle with the alphabet soup</summary>
        AlphabetSoup = 0x1,

        ///<summary>Prints the words list</summary>
        Words = 0x2,

        ///<summary>Prints the soup and the words list</summary>
        Normal = AlphabetSoup | Words,

        ///<summary>Remarks the solution inside the alphabet soup</summary>
        Solution = 0x4,

        ///<summary>Prints the soup, the words list and remarks the solution</summary>
        All = AlphabetSoup | Words | Solution
    }
}
