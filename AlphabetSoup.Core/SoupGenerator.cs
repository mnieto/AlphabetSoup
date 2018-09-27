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

        /// <summary>
        /// <see cref="Options"/> used to configure this generator
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
            if (Options?.Words.Count() > 0) {
                Words = Options.Words;
            } else {
                Words = data.Lemmata;
            }
            Letters = data.Letters;
            AllowedDirections = GetDirections(options.AllowedDirections);
        }

        /// <summary>
        /// Initialize the Alphabet Soup with random data, with the size and language settings specified in the <see cref="Options"/>
        /// </summary>
        /// <returns>Initialized <see cref="Soup"/> with random data</returns>
        internal SoupGenerator Init() {
            Soup = new Soup();
            Soup.Matrix = new int[Options.Size, Options.Size];
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
                bool added = false;
                do {
                    int index = random.Next(Words.Count - 1);
                    string word = Words[index];
                    if (!Soup.UsedWords.ContainsKey(word)) {
                        Word wordEntry = new Word {
                            X = random.Next(Options.Size - 1),
                            Y = random.Next(Options.Size - 1),
                            Direction = AllowedDirections[random.Next(AllowedDirections.Length - 1)],
                            Name = word
                        };
                        if (!IsOverlapped(wordEntry))
                            added = true;
                    }
                } while (!added);
            }
            return Soup;
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
        /// A word overlaps any of the already positioned word?
        /// </summary>
        /// <param name="wordEntry"><see cref="Word"/> to be positioned in the <see cref="Soup"/></param>
        /// <returns><c>true</c> if is completely overlapped</returns>
        protected bool IsOverlapped(Word wordEntry) {
            return false;
        }

        /// <summary>
        /// Reads de data associated to a language
        /// </summary>
        /// <returns>A initialized <see cref="LanguageData"/></returns>
        protected LanguageData LoadWordsFromFile() {
            var configuration = new ConfigurationManager(new System.IO.Abstractions.FileSystem());
            return configuration.ReadLanguageData(Options.CultureCode, Options.Words != null && Options.Words.Count() == 0);
        }
    }
}
