using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphabetSoup.Core {

    /// <summary>
    /// Determines if the length from the origin coordinate to the end of the <see cref="Soup.Matrix"/> (in the <see cref="Directions"/> direction is equal or greather than the length of the word
    /// </summary>
    public class HaveSpace : IRule {
        public string Name => "Check if words will be truncated because there is not enough space from origin coordinate to the end of the soup";

        public bool Check(Soup soup, WordEntry entry) {
            var boundaries = new BoundariesManager(NullLogger<BoundariesManager>.Instance);
            boundaries.Soup = soup;
            return boundaries.Check(entry);
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
                            return false;
                    } else {
                        if (IsOverlapped(entry, item))
                            return false;
                    }
                }
            }
            return true;
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

    /// <summary>
    /// Checks the new word was not previously selected
    /// </summary>
    public class NotUsed : IRule {
        public string Name => "Avoid use repeated word in same soup";

        public bool Check(Soup soup, WordEntry entry) {
            return !soup.UsedWords.Keys.Contains(entry.Name);
        }
    }

    /// <summary>
    /// Check the word length is between <see cref="Options.MinLength"/> and <see cref="Options.MaxLength"/>
    /// </summary>
    public class MatchLengthRange : IRule {
        public string Name => "The word length is between options min length and max length";
        private int _min;
        private int _max;
        public MatchLengthRange(int min, int max) {
            _min = min;
            _max = max;
        }

        public bool Check(Soup soup, WordEntry entry) {
            return entry.Name.Length >= _min && entry.Name.Length <= _max;
        }
    }

}
