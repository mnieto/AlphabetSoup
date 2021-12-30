using System.Collections.Generic;

namespace AlphabetSoup.Core {
    public interface IIntersectionManager {
        WordEntry Candidate { get; set; }
        IntersectionManager.Range CandidateRange { get; set; }
        List<IntersectionManager.CommonLetter> CommonLetters { get; }
        WordEntry Existing { get; set; }
        IntersectionManager.Range ExistingRange { get; set; }
        bool Intersects { get; }
        bool Overlaps { get; }
        Soup Soup { get; set; }

        void Check(WordEntry existing, WordEntry candidate);
        bool GetCommonLetters();
        IEnumerable<Point> GetIntersection();
        bool IntersectsWithOthers(WordEntry candidate, IEnumerable<WordEntry> entries);
        WordEntry RepositionEntry();
    }
}