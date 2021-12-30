namespace AlphabetSoup.Core {
    public interface IBoundariesManager {
        bool Check(WordEntry entry);
        Point GetDelta(WordEntry entry);
        Soup Soup { get; set; }
    }
}