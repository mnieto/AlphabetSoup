using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Determines if the length from the origin coordinate to the end of the <see cref="Soup.Matrix"/> (in the <see cref="Directions"/> direction is equal or greather than the length of the word
    /// </summary>
    public class HaveSpace : IRule {
        public string Name => "Check if words will be truncated because there is not enough space from origin coordinate to the end of the soup";

        public bool Check(Soup soup, WordEntry entry) {
            bool haveHorizontalSpace = false;
            bool haveVerticalSpace = false;
            int wordLength = entry.Name.Length;

            //X
            switch (entry.Direction) {
                case Directions.N:
                case Directions.S:
                    haveHorizontalSpace = true;
                    break;
                case Directions.E:
                case Directions.NE:
                case Directions.SE:
                    haveHorizontalSpace = soup.Matrix.GetLength(0) - wordLength >= entry.X;
                    break;
                case Directions.W:
                case Directions.NW:
                case Directions.SW:
                    haveHorizontalSpace = entry.X - wordLength + 1 >= 0;
                    break;
            }

            //Y
            switch (entry.Direction) {
                case Directions.E:
                case Directions.W:
                    haveVerticalSpace = true;
                    break;
                case Directions.N:
                case Directions.NE:
                case Directions.NW:
                    haveVerticalSpace = soup.Matrix.GetLength(1) - wordLength >= entry.Y;
                    break;
                case Directions.S:
                case Directions.SE:
                case Directions.SW:
                    haveVerticalSpace = entry.Y - wordLength >= 0;
                    break;
            }

            return haveHorizontalSpace && haveVerticalSpace;
        }
    }

    /// <summary>
    /// A word overlaps any of the already positioned word?
    /// </summary>
    public class NotOverlapped : IRule {
        public string Name => "Check if new words will be completely overlapped by an previously added word";

        public bool Check(Soup soup, WordEntry entry) {
            foreach (WordEntry item in soup.UsedWords.Values) {
                if (item.Direction.SameDirection(entry.Direction)) {
                    if (item.Name.Length >= entry.Name.Length) {
                        if (IsOverlapped(item, entry))
                            return true;
                    } else {
                        if (IsOverlapped(entry, item))
                            return true;
                    }
                }
            }
            return false;
        }

        private bool IsOverlapped(WordEntry bigWord, WordEntry smallWord) {
            var big = bigWord.AbsoluteOrigin();
            var small = smallWord.AbsoluteOrigin();
            bool overlapp = false;
            if (bigWord.Direction.MovesHorizontal()) {
                overlapp = small.X >= big.X && small.X + smallWord.Name.Length <= big.X + bigWord.Name.Length;
            }
            if (!overlapp && bigWord.Direction.MovesVertical()) {
                overlapp = small.Y >= big.Y && small.Y + smallWord.Name.Length <= big.Y + bigWord.Name.Length;
            }
            return overlapp;
        }

    }
}
