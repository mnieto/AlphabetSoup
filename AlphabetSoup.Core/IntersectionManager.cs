using System;
using System.Collections.Generic;
using System.Text;

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
        /// Ctor
        /// </summary>
        /// <param name="existing"><see cref="WordEntry"/> already set in the <see cref="Soup"/></param>
        /// <param name="candidate"><see cref="WordEntry"/> to test if can be set in the <see cref="Soup"/></param>
        public IntersectionManager(WordEntry existing, WordEntry candidate) {
            //TODO: Store information about the Soup sizing so can reposition taking in account the matrix boundaries 
            Existing = existing;
            Candidate = candidate;
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
        public bool HasCommonLetters() {
            if (Existing.Direction.SameDirection(Candidate.Direction)) {
                //TODO: Comprobar desde el final de una con el principio de la otra y viceversa. Tener en cuenta si la dirección es normal o reverse
                throw new NotImplementedException();
            } else {
                for (int i = 0; i < Existing.Name.Length; i++) {
                    int pos = Candidate.Name.IndexOf(Existing.Name[i]);
                    if (pos >= 0) {
                        ExistingRange.Init = i;
                        ExistingRange.Length = 1;
                        CandidateRange.Init = pos;
                        CandidateRange.Length = 1;
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Compute and set the new <see cref="WordEntry.Origin"/> of the <see cref="Candidate"/> entry, so this entry intersects in the common letter
        /// </summary>
        /// <exception cref="InvalidOperationException">If both entries do not intersect or do not have common letters</exception>
        public void RepositionEntry() {
            if (!Intersects)
                throw new InvalidOperationException("Can't reposition an entry that do not intersects with others");
            if (!HasCommonLetters())
                throw new InvalidOperationException("Can't reposition an entry without common letters");
            if (Overlaps) {
                throw new NotImplementedException();
            } else {
                Point target = Existing.Coordinate(ExistingRange.Init);
                Point delta = target.Delta(Candidate.Coordinate(CandidateRange.Init));
                //TODO: If the resulting origin in the translated candidate is out of the Soup bounds, try to intersect with the next common letter
                //      If there are not more letters return false to state that has not been possible to reposition the candidate entry. Otherwise return true
                //      The list of common letters only is useful when the words intersect. 
                //      When they overlap (the same direction), its necessary to maintain the range management
                Candidate.Translate(delta);
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
    }
}
