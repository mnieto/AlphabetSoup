using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    public enum PrintOptions {
        None = 0x0,
        AlphabetSoup = 0x1,
        Words = 0x2,
        Normal = AlphabetSoup | Words,
        Solution = 0x4,
        All = AlphabetSoup | Words | Solution
    }
}
