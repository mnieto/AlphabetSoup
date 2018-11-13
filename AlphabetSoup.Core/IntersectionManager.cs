using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlphabetSoup.Core
{
    /// <summary>
    /// Information and management about the intersection of two <see cref="WordEntry"/> entries
    /// </summary>
    public class IntersectionManager : List<Point> {

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
        private Soup Soup{ get; set; }

        /// <summary>
        /// List of common letters and their positions in the existing and candidate words
        /// </summary>
        public List<CommonLetter> CommonLetters { get; private set; } = new List<CommonLetter>();

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="existing"><see cref="WordEntry"/> already set in the <see cref="Soup"/></param>
        /// <param name="candidate"><see cref="WordEntry"/> to test if can be set in the <see cref="Soup"/></param>
        /// <param name="size"><see cref="Soup"/> size to check boundaries while finding the correct position</param>
        public IntersectionManager(WordEntry existing, WordEntry candidate, Soup soup) {
            Existing = existing;
            Candidate = candidate;
            Soup = soup;
            GetIntersection();
        }

        /// <summary>
        /// Returns a list of coordinates where both entries intersects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point> GetIntersection() {
            Clear();
            ExistingRange = new Range();
            CandidateRange = new Range();
            for (int i = 0; i < Existing.Name.Length; i++) {
                for (int j = 0; j < Candidate.Name.Length; j++) {
                    if (Existing.Coordinate(i) == Candidate.Coordinate(j)) {
                        Add(Existing.Coordinate(i));
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// Returns <c>true</c> if both <see cref="WordEntry"/> entries has almost one letter in common
        /// </summary>
        public bool GetCommonLetters() {
            if (Existing.Direction.SameDirection(Candidate.Direction)) {
                //TODO: Comprobar desde el final de una con el principio de la otra y viceversa. Tener en cuenta si la dirección es normal o reverse
                throw new NotImplementedException();
            } else {
                for (int i = 0; i < Existing.Name.Length; i++) {
                    int pos = Candidate.Name.IndexOf(Existing.Name[i]);
                    if (pos >= 0) {
                        CommonLetters.Add(new CommonLetter {
                            Letter = Existing.Name[i],
                            ExistingPos = i,
                            CandidatePos = pos
                        });
                        //ExistingRange.Init = i;
                        //ExistingRange.Length = 1;
                        //CandidateRange.Init = pos;
                        //CandidateRange.Length = 1;
                        //return true;
                    }
                }
                return CommonLetters.Count > 0;
            }
        }

        /// <summary>
        /// Compute and set the new <see cref="WordEntry.Origin"/> of the <see cref="Candidate"/> entry, so this entry intersects in the common letter
        /// </summary>
        /// <exception cref="InvalidOperationException">If both entries do not intersect or do not have common letters</exception>
        public bool RepositionEntry() {
            if (!Intersects)
                throw new InvalidOperationException("Can't reposition an entry that do not intersects with others");
            if (!GetCommonLetters())
                throw new InvalidOperationException("Can't reposition an entry without common letters");
            if (Overlaps) {
                throw new NotImplementedException();
            } else {
                int i = 0;
                bool insideBoundaries = false;
                do {
                    Point target = Existing.Coordinate(CommonLetters[i].ExistingPos);
                    Point delta = target.Delta(Candidate.Coordinate(CommonLetters[i].CandidatePos));
                    Candidate.Translate(delta);
                    insideBoundaries = CheckBoundaries(Candidate.Origin);
                } while (!insideBoundaries && i < CommonLetters.Count);
                return insideBoundaries && !IntersectsWithOthers(Soup.UsedWords.Values);
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the <see cref="Candidate"/> entry intersects with other entries besides the <see cref="Existing"/>
        /// </summary>
        /// <param name="entries">List of entries already set up in the <see cref="Soup"/></param>
        public bool IntersectsWithOthers(IEnumerable<WordEntry> entries) {
            foreach (WordEntry entry in entries) {
                if (entry.Name != Existing.Name && entry.IntersectWith(Candidate)) {
                    return true;
                }
            }
            return false;
        }
        private bool CheckBoundaries(Point origin) {
            var size = new Point(Soup.Matrix.GetUpperBound(0), Soup.Matrix.GetUpperBound(1));
            return origin.X >= 0 && origin.X < size.X &&
                   origin.Y >= 0 && origin.Y < size.Y; 
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

        public class CommonLetter {
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
        }
    }
}
