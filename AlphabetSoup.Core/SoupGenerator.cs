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
            Soup.Matrix = new char[Options.Size, Options.Size];
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
                        if (!IsOverlapped(wordEntry) && HaveSpace(wordEntry))
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
            foreach (Word item in Soup.UsedWords.Values) {
                if (item.Direction.SameDirection(wordEntry.Direction)) {
                    if (item.Name.Length >= wordEntry.Name.Length) {
                        if (IsOverlapped(item, wordEntry))
                            return true;
                    } else {
                        if (IsOverlapped(wordEntry, item))
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the length from the origin coordinate to the end of the <see cref="Soup.Matrix"/> (in the <see cref="Directions"/> direction is equal or greather than the length of the word
        /// </summary>
        /// <param name="wordEntry"><see cref="Word"/> to be positioned in the <see cref="Soup"/></param>
        protected bool HaveSpace(Word wordEntry) {
            bool haveHorizontalSpace = false;
            bool haveVerticalSpace = false;
            int wordLength = wordEntry.Name.Length;

            //X
            switch (wordEntry.Direction) {
                case Directions.N:
                case Directions.S:
                    haveHorizontalSpace = true;
                    break;
                case Directions.E:
                case Directions.NE:
                case Directions.SE:
                    haveHorizontalSpace = Soup.Matrix.GetLength(0) - wordLength >= wordEntry.X;
                    break;
                case Directions.W:
                case Directions.NW:
                case Directions.SW:
                    haveHorizontalSpace = wordEntry.X - wordLength >= 0;
                    break;
            }

            //Y
            switch (wordEntry.Direction) {
                case Directions.E:
                case Directions.W:
                    haveVerticalSpace = true;
                    break;
                case Directions.N:
                case Directions.NE:
                case Directions.NW:
                    haveVerticalSpace = Soup.Matrix.GetLength(1) - wordLength >= wordEntry.Y;
                    break;
                case Directions.S:
                case Directions.SE:
                case Directions.SW:
                    haveVerticalSpace = wordEntry.Y - wordLength >= 0;
                    break;
            }

            return haveHorizontalSpace && haveVerticalSpace;
        }

        /// <summary>
        /// Reads de data associated to a language
        /// </summary>
        /// <returns>A initialized <see cref="LanguageData"/></returns>
        protected LanguageData LoadWordsFromFile() {
            var configuration = new ConfigurationManager(new System.IO.Abstractions.FileSystem());
            return configuration.ReadLanguageData(Options.CultureCode, Options.Words != null && Options.Words.Count() == 0);
        }


        private bool IsOverlapped(Word bigWord, Word smallWord) {
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
