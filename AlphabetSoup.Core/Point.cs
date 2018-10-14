using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AlphabetSoup.Core {

    [DebuggerDisplay("X: {X}, Y: {Y}")]
    public struct Point {
        public int X;
        public int Y;

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return $"X: {X}, Y: {Y}";
        }
    }
}
