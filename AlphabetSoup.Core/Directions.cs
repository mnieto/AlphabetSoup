using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphabetSoup.Core
{
    [Flags]
    public enum Directions
    {
        /// <summary>From top to down </summary>
        N = 1,

        /// <summary>Diagonal from top to down-right</summary>
        NE = 2,

        /// <summary>From left to right</summary>
        E = 4,
        
        /// <summary>Diagonal from bottom to top-right</summary>
        SE = 8,

        /// <summary>From bottom to top (reverse)</summary>
        S = 16,

        /// <summary>Diagonal from bottom to top-left (reverse)</summary>
        SW = 32,
        
        /// <summary>From right to left (reverse)</summary>
        W = 64,

        /// <summary>Diagonal from top to bottom-left (reverse)</summary>
        NW = 128
    }

    public static class DirectionExtensions {

        /// <summary>
        /// Returns <c>true</c> if the current direction and the <paramref name="other"/> direction are the same or equivalent (same direction, opposite way)
        /// </summary>
        public static bool SameDirection(this Directions thisDirection, Directions other) {
            if (thisDirection == other)
                return true;
            int max = Math.Max((int)thisDirection, (int)other);
            int min = Math.Min((int)thisDirection, (int)other);
            return (max >> 4 == min);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="thisDirection"/> is reverse; <c>false</c> if is straight direction
        /// </summary>
        public static bool IsReverse(this Directions thisDirection) {
            return ((int)thisDirection) >= 16;
        }

        /// <summary>
        /// Returns <c>true</c> if the X coordinate increments (or decrements) to write the word
        /// </summary>
        public static bool MovesHorizontal(this Directions thisDirection) {
            Directions[] testDirections = { Directions.E, Directions.W, Directions.NE, Directions.NW, Directions.SE, Directions.SW };
            return testDirections.Contains(thisDirection);
        }

        /// <summary>
        /// Returns <c>true</c> if the Y coordinate increments (or decrements) to write the word
        /// </summary>
        public static bool MovesVertical(this Directions thisDirection) {
            Directions[] testDirections = { Directions.N, Directions.S, Directions.NE, Directions.NW, Directions.SE, Directions.SW };
            return testDirections.Contains(thisDirection);
        }

    }
}
