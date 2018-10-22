using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AlphabetSoup.Core {

    [DebuggerDisplay("({X}, {Y})")]
    public struct Point : IEquatable<Point> {
        public int X;
        public int Y;

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public bool Equals(Point other) {
            if (ReferenceEquals(other, null)) return false;
            return X == other.X && Y == other.Y;
        }


        public override bool Equals(object obj) {
            if (obj is Point) {
                Point other = (Point)obj;
                return Equals(other);
            }
            throw new ArgumentException($"{nameof(obj)} must be of type Point");
        }
        public override int GetHashCode() {
            unchecked {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ X.GetHashCode();
                hash = (hash * 16777619) ^ Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString() {
            return $"X: {X}, Y: {Y}";
        }

        public static bool operator== (Point pt1, Point pt2) {
            return pt1.X == pt2.X && pt1.Y == pt2.Y;
        }

        public static bool operator !=(Point pt1, Point pt2) {
            return pt1.X != pt2.X || pt1.Y != pt2.Y;
        }
    }
}
