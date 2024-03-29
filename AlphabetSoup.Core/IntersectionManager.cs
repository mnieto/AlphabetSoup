﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace AlphabetSoup.Core
{
    /// <summary>
    /// Information and management about the intersection of two <see cref="WordEntry"/> entries
    /// </summary>
    public class IntersectionManager : List<Point>, IIntersectionManager {

        /// <summary>
        /// Returns <c>true</c> if the <see cref="Existing"/> and the <see cref="Candidate"/> entries intersects in one point (one letter)
        /// </summary>
        public bool Intersects => Count > 0;

        /// <summary>
        /// Returns <c>true</c> if the <see cref="Existing"/> and the <see cref="Candidate"/> entries has the same or equivalent direction and overlaps in more than one position
        /// </summary>
        public bool Overlaps => Count > 1;

        /// <summary>
        /// <see cref="WordEntry"/> already set in the <see cref="Soup"/>
        /// </summary>
        public WordEntry Existing { get; set; }

        /// <summary>
        /// <see cref="WordEntry"/> to test if can be set in the <see cref="Soup"/>
        /// </summary>
        public WordEntry Candidate { get; set; }

        /// <summary>
        /// Position and length of the <see cref="WordEntry.Name"/> substring that intersects with the <see cref="Candidate"/> entry
        /// </summary>
        public Range ExistingRange { get; set; }

        /// <summary>
        /// Position and length of the <see cref="WordEntry.Name"/> substring that intersects with the <see cref="Existing"/> entry
        /// </summary>
        public Range CandidateRange { get; set; }

        /// <summary>
        /// <see cref="Soup"/> where put the new <see cref="WordEntry"/>
        /// </summary>
        public Soup Soup { get; set; }

        /// <summary>
        /// List of common letters and their positions in the existing and candidate words
        /// </summary>
        public List<CommonLetter> CommonLetters { get; private set; } = new List<CommonLetter>();

        private ILogger<IntersectionManager> Logger { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public IntersectionManager(ILogger<IntersectionManager> logger) {
            Logger = logger;
        }

        /// <summary>
        /// Initialize the words to check if have intersection
        /// </summary>
        /// <param name="existing"><see cref="WordEntry"/> already set in the <see cref="Soup"/></param>
        /// <param name="candidate"><see cref="WordEntry"/> to test if can be set in the <see cref="Soup"/></param>
        public void Check(WordEntry existing, WordEntry candidate) {
            if (Soup == null) {
                throw new InvalidOperationException($"{nameof(Soup)} must be initialized");
            }
            Existing = existing ?? throw new ArgumentNullException(nameof(existing));
            Candidate = candidate ?? throw new ArgumentNullException(nameof(candidate));
            if (Logger.IsEnabled(LogLevel.Trace)) Logger.LogDebug($"Comparing to ({Existing})");
            GetIntersection();
        }

        /// <summary>
        /// Returns a list of coordinates where both entries intersects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point> GetIntersection() {
            Clear();
            CommonLetters.Clear();
            ExistingRange = new Range();
            CandidateRange = new Range();
            for (int i = 0; i < Existing.Name.Length; i++) {
                for (int j = 0; j < Candidate.Name.Length; j++) {
                    if (Existing.Coordinate(i) == Candidate.Coordinate(j)) {
                        Add(Existing.Coordinate(i));
                        Logger.LogTrace($"  {Existing.Coordinate(i)}");
                    }
                }
            }
            Logger.LogDebug($"Comparing ({Existing}) with candidate ({Candidate}): {Count} intersection(s) found");
            return this;
        }

        /// <summary>
        /// Returns <c>true</c> if both <see cref="WordEntry"/> entries has almost one letter in common
        /// </summary>
        public bool GetCommonLetters() {
            if (Existing.Direction.SameDirection(Candidate.Direction)) {
                var result = FindCommonLetters(Existing, Candidate);
                if (result.hasCommon) {
                    ExistingRange = result.left;
                    CandidateRange = result.right;
                } else {
                    result = FindCommonLetters(Candidate, Existing);
                    if (result.hasCommon) {
                        ExistingRange = result.right;
                        CandidateRange = result.left;
                    }
                }
                if (result.hasCommon) {
                    for (int i = ExistingRange.Init; i < ExistingRange.Init + ExistingRange.Length; i++) {
                        CommonLetters.Add(new CommonLetter {
                            Letter = Existing.Name[i],
                            ExistingPos = i,
                            CandidatePos = CandidateRange.Init + i
                        });
                    }
                    return true;
                }
                return false;
            } else {
                for (int i = 0; i < Existing.Name.Length; i++) {
                    int pos = Candidate.Name.IndexOf(Existing.Name[i]);
                    if (pos >= 0) {
                        CommonLetters.Add(new CommonLetter {
                            Letter = Existing.Name[i],
                            ExistingPos = i,
                            CandidatePos = pos
                        });
                    }
                }
                return CommonLetters.Count > 0;
            }
        }

        /// <summary>
        /// Compute and set the new <see cref="WordEntry.Origin"/> of the <see cref="Candidate"/> entry, so this entry intersects in the common letter
        /// </summary>
        /// <exception cref="InvalidOperationException">If both entries do not intersect or do not have common letters</exception>
        public WordEntry RepositionEntry() {
            if (!Intersects)
                throw new InvalidOperationException("Can't reposition an entry that do not intersects with others");
            if (!GetCommonLetters()) {
                Logger.LogWarning($"Words {Existing} and {Candidate} intersect but don't have common letters");
                return null;
            }
            bool reposition = false;
            WordEntry repositionedWord = null;
            if (Overlaps) {
                Logger.LogInformation($"Words {Existing} and {Candidate} overlap {Count} common letters");
                Point target = Existing.Coordinate(ExistingRange.Init);
                int increment = Candidate.Direction.IsReverse() ? 1 : 0;
                Point delta = target.Delta(Candidate.Coordinate(CandidateRange.Init + increment));
                repositionedWord = Candidate.Translate(delta);
                bool insideBoundaries = CheckBoundaries(repositionedWord.Origin);
                reposition = insideBoundaries && !IntersectsWithOthers(repositionedWord, Soup.UsedWords.Values);
            } else {
                int i = 0;
                bool insideBoundaries = false;
                Logger.LogInformation($"Candidate intersects at ({this[0]}) with ({Existing})");
                do {
                    Point target = Existing.Coordinate(CommonLetters[i].ExistingPos);
                    Point delta = target.Delta(Candidate.Coordinate(CommonLetters[i].CandidatePos));
                    repositionedWord = Candidate.Translate(delta);
                    Logger.LogDebug($"Try to reposition to: {repositionedWord}");
                    insideBoundaries = CheckBoundaries(repositionedWord.Origin);
                    i++;
                } while (!insideBoundaries && i < CommonLetters.Count);
                reposition = insideBoundaries && !IntersectsWithOthers(repositionedWord, Soup.UsedWords.Values);
            }
            if (reposition) {
                Logger.LogWarning($"Candidate repositioned: {repositionedWord}");
                return repositionedWord;
            }
            return null;

        }

        /// <summary>
        /// Returns <c>true</c> if the <see cref="Candidate"/> entry intersects with other entries besides the <see cref="Existing"/>
        /// </summary>
        /// <param name="candidate">Candidate word to check with existing entries</param>
        /// <param name="entries">List of entries already set up in the <see cref="Soup"/></param>
        public bool IntersectsWithOthers(WordEntry candidate, IEnumerable<WordEntry> entries) {
            foreach (WordEntry entry in entries) {
                if (entry.Name != Existing.Name && entry.IntersectWith(candidate)) {
                    Logger.LogDebug($"Also intersects with {entry}");
                    return true;
                }
            }
            return false;
        }
        private bool CheckBoundaries(Point origin) {
            var size = new Point(Soup.Matrix.GetUpperBound(0), Soup.Matrix.GetUpperBound(1));
            bool inside = origin.X >= 0 && origin.X < size.X &&
                          origin.Y >= 0 && origin.Y < size.Y;
            if (!inside)
                Logger.LogDebug("Is not in boundaries");
            return inside;
        }

        private (Range left, Range right, bool hasCommon) FindCommonLetters(WordEntry a, WordEntry b) {
            int minLength = Math.Min(a.Name.Length, b.Name.Length);
            string nameA = a.Direction.IsReverse() ? Reverse(a.Name) : a.Name;
            string nameB = b.Direction.IsReverse() ? Reverse(b.Name) : b.Name;
            for (int i = 1; i < minLength; i++) {
                string e = nameA.Substring(nameA.Length - i);
                string c = nameB.Substring(0, i);
                if (e == c) {
                    int leftInit = a.Direction.IsReverse() ? 0 : nameA.Length - i;
                    int rightInit = b.Direction.IsReverse() ? nameB.Length - i : 0;
                    return (left: new Range { Init = leftInit, Length = i },
                            right: new Range { Init = rightInit, Length = i },
                            hasCommon: true
                    );
                }
            }
            return (left: null, right: null, hasCommon: false);
        }

        private string Reverse(string value) {
            return new string(value.Reverse().ToArray());
        }

        /// <summary>
        /// Specify the position and length of a substring
        /// </summary>
        public class Range {

            /// <summary>
            /// Index of the substring beginning
            /// </summary>
            public int Init { get; set; }

            /// <summary>
            /// Length of the substring
            /// </summary>
            public int Length { get; set; }

        }

        public class CommonLetter : IEquatable<CommonLetter> {
            /// <summary>
            /// Common letter 
            /// </summary>
            public char Letter { get; set; }

            /// <summary>
            /// In which position is the letter in the existing word
            /// </summary>
            public int ExistingPos { get; set; }

            /// <summary>
            /// In which position is the letter in the candidate word
            /// </summary>
            public int CandidatePos { get; set; }

            public bool Equals(CommonLetter other) {
                return Letter == other.Letter &&
                       ExistingPos == other.ExistingPos &&
                       CandidatePos == other.CandidatePos;
            }

            public static bool operator ==(CommonLetter left, CommonLetter right) {
                return left.Letter == right.Letter &&
                       left.ExistingPos == right.ExistingPos &&
                       left.CandidatePos == right.CandidatePos;
            }

            public static bool operator !=(CommonLetter left, CommonLetter right) {
                return left.Letter != right.Letter ||
                       left.ExistingPos != right.ExistingPos ||
                       left.CandidatePos != right.CandidatePos;
            }

            public override bool Equals(object obj) {
                if (obj == null || !(obj is CommonLetter))
                    throw new InvalidCastException($"{nameof(obj)} must be not null and of type {GetType().Name}");
                return Equals((CommonLetter)obj);
            }

            public override int GetHashCode() {
                int hash = 17;
                hash = hash * 23 + Letter;
                hash = hash * 23 + ExistingPos;
                hash = hash * 23 + CandidatePos;
                return hash;
            }
        }
    }
}
