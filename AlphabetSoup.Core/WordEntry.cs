using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Word used in alphabet soup. It describes the word and location inside the soup
    /// </summary>
    public class WordEntry {

        /// <summary>
        /// X origin coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y origin coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Direction to write to
        /// </summary>
        public Directions Direction { get; set; }

        /// <summary>
        /// Word itself
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the coordinate of the first letter of the word, independently if it's straight direction or reverse direction
        /// </summary>
        public Point AbsoluteOrigin() {
            return Direction.IsReverse() ? EndingCoordinate() : new Point(X, Y);
        }

        public override string ToString() {
            return $"{Name} at ({X}, {Y}) with {Direction}";
        }

        /// <summary>
        /// Gets the origin coordinate of the word
        /// </summary>
        public Point Origin => new Point(X, Y);

        /// <summary>
        /// Returns the coordinate of the letter for n-th position
        /// </summary>
        /// <param name="position">position of the letter to get the coordinate. Position=0 will return Origin</param>
        /// <returns></returns>
        public Point Coordinate(int position) {
            switch (Direction) {
                case Directions.E:
                    return new Point(X + position, Y);
                case Directions.N:
                    return new Point(X, Y + position);
                case Directions.NE:
                    return new Point(X + position, Y + position);
                case Directions.NW:
                    return new Point(X - position, Y + position);
                case Directions.S:
                    return new Point(X, Y - position);
                case Directions.SE:
                    return new Point(X + position, Y - position);
                case Directions.SW:
                    return new Point(X - position, Y - position);
                case Directions.W:
                    return new Point(X - position, Y);
                default:
                    throw new InvalidOperationException("Direction not supported");
            }
        }


        /// <summary>
        /// Returns de end coordinate, that is, the coordinate of the last letter
        /// </summary>
        /// <returns></returns>
        public Point EndingCoordinate() {
            return Coordinate(Name.Length - 1);
        }
    }
}
