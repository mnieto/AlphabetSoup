using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("AlphabetSoup.Core.Test")]

namespace AlphabetSoup.Core {

    /// <summary>
    /// Generate a new alphabet soup following the directives set in the <see cref="Options"/>
    /// </summary>
    internal class SoupGenerator {

        /// <summary>
        /// List of words used as dictionary to pick words from
        /// </summary>
        internal List<string> Words { get; private set; }

        /// <summary>
        /// Allowed letters to randomly fill the soup initially
        /// </summary>
        internal char[] Letters { get; private set; }

        /// <summary>
        /// Allowed <see cref="Directions"/> to be used to generate words. Generator will randomly select a direction from this array
        /// </summary>
        internal Directions[] AllowedDirections { get; private set; }

        internal IEnumerable<IRule> Rules { get; private set; }

        /// <summary>
        /// <see cref="Options"/> used to configure this generator. 
        /// After <see cref="SoupGenerator.SoupGenerator(Options)"/> constructor, any changes in the options will be ignored
        /// </summary>
        /// 
        internal Options Options { get; private set; }

        /// <summary>
        /// Generated aphabet <see cref="Soup"/>
        /// </summary>
        public Soup Soup { get; protected set; }

        private Random random = new Random();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"><see cref="Options"/> used to configure this generator</param>
        internal SoupGenerator(Options options) {
            Options = options;
            LanguageData data = LoadWordsFromFile();
            if (Options.Words == null) {
                Words = data.Lemmata;
            } else {
                Words = Options.Words;
            }
            Letters = data.Letters;
            AllowedDirections = GetDirections(options.AllowedDirections);
            if (options.Rules != null && options.Rules.Count > 0)
                Rules = options.Rules;
            else
                Rules = StandardRules();
        }

        /// <summary>
        /// Initialize the Alphabet Soup with random data, with the size and language settings specified in the <see cref="Options"/>
        /// </summary>
        /// <returns>Initialized <see cref="Soup"/> with random data</returns>
        internal SoupGenerator Init() {
            Soup = new Soup();
            Soup.Matrix = new char[Options.Size, Options.Size];
            Soup.ShadowMatrix = new bool[Options.Size, Options.Size];
            for (int x = 0; x < Options.Size ; x++) {
                for (int y = 0; y < Options.Size; y++) {
                    int i = random.Next(Letters.Length - 1);
                    Soup.Matrix[x, y] = Letters[i];
                }
            }
            return this;
        }

        /// <summary>
        /// Fill the alphabet soup with random words in random coordinates and directions
        /// </summary>
        /// <returns>The reated <see cref="Soup"/></returns>
        internal Soup Create() {
            for (int i = 0; i < Options.NumWords; i++) {
                bool failed = false;
                WordEntry wordEntry = null;
                do {
                    failed = false;
                    wordEntry = NextEntry();

                    foreach (IRule rule in Rules) {
                        if (!rule.Check(Soup, wordEntry)) {
                            failed = true;
                            break;
                        }
                    }
                } while (failed);
                System.Diagnostics.Debug.Assert(wordEntry != null);
                AddWord(wordEntry);
            }
            return Soup;
        }

        /// <summary>
        /// Generates an new <see cref="WordEntry"/> with random name, position, and direction
        /// </summary>
        internal WordEntry NextEntry() {
            int index = random.Next(Words.Count - 1);
            string word = Words[index];

            return new WordEntry {
                X = random.Next(Options.Size - 1),
                Y = random.Next(Options.Size - 1),
                Direction = AllowedDirections[random.Next(AllowedDirections.Length - 1)],
                Name = word
            };
        }

        private void AddWord(WordEntry wordEntry) {
            for (int i = 0; i < wordEntry.Name.Length; i++) {
                char c = wordEntry.Name[i];
                Point pt = wordEntry.Coordinate(i);
                try {
                    Soup.Matrix[pt.X, pt.Y] = c;
                    Soup.ShadowMatrix[pt.X, pt.Y] = true;
                } catch (Exception ex) {
                    System.Diagnostics.Debug.Print($"{ex.Message} at ({pt.X}, {pt.Y}) with entry: {wordEntry}");
                    throw;
                }
            }
            Soup.UsedWords.Add(wordEntry.Name, wordEntry);
        }

        /// <summary>
        /// Gets an array with individual directions from a <see cref="Directions"/> with all the allowed directions bits set
        /// </summary>
        /// <param name="directions">Allowed directions</param>
        protected Directions[] GetDirections(Directions directions) {
            List<Directions> result = new List<Directions>();
            foreach(Directions dir in Enum.GetValues(typeof(Directions))) {
                if ((directions & dir) == dir) {
                    result.Add(dir);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Reads de data associated to a language
        /// </summary>
        /// <returns>A initialized <see cref="LanguageData"/></returns>
        protected LanguageData LoadWordsFromFile() {
            var configuration = new ConfigurationManager(new System.IO.Abstractions.FileSystem());
            return configuration.ReadLanguageData(Options.CultureCode, Options.Words == null || Options.Words.Count() == 0);
        }

        protected IEnumerable<IRule> StandardRules() {
            yield return new NotUsed();
            yield return new HaveSpace();
            yield return new NotOverlapped();
            if (Options.MinLength != 0 || Options.MaxLength != 0) {
                yield return new MatchLengthRange(Options.MinLength, Options.MaxLength == 0 ? int.MaxValue : Options.MaxLength);
            }
        }


    }
}
