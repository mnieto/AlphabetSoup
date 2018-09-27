using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core
{
    [Flags]
    public enum Directions
    {
        N = 1,
        NE = 2,
        E = 4,
        SE = 8,
        S = 16,
        SW = 32,
        W = 64,
        NW = 128
    }
}
