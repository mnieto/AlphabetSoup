using AlphabetSoup.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AlpabetSoup.Core.Test {
    public class SoupGeneratorTest {

        List<string> words;

        public SoupGeneratorTest() {
            words = new List<string> { "alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho", "tau", "upsilon", "phi", "chi", "psi", "omega" };
        }

        [Fact]
        public void MembersAreInitializedAfterConstructorInvoke() {
            var sut = InitGenerator();
            Assert.Equal(words.Count, sut.Words.Count);
            Assert.Equal(27, sut.Letters.Length);
            Assert.Single(sut.AllowedDirections);
        }

        [Fact]
        public void MatrixIsInitializedAfterInit() {
            var sut = InitGenerator();
            sut.Init();

            Random rnd = new Random();
            char c = sut.Soup.Matrix[rnd.Next(29), rnd.Next(29)];
            Assert.True(char.IsLetter(c));
            Assert.True(char.IsUpper(c));
            Assert.True(sut.Letters.ToList().IndexOf(c) != -1);
        }

        [Fact]
        public void WordsAreGeneratedAndPositioned() {
            var generator = InitGenerator();
            var sut = generator.Init().Create();

            Assert.Equal(generator.Options.NumWords, sut.UsedWords.Count);
        }

        [Fact]
        public void GeneratorHasDefaultRules() {
            var generator = InitGenerator();

            Assert.Equal(3, generator.Rules.Count);
        }

        private SoupGenerator InitGenerator() {
            return new SoupGenerator(new Options {
                CultureCode = "es-es",
                NumWords = 10,
                Size = 30,
                Words = words,
                AllowedDirections = Directions.E
            });
        }

        protected string ReadTestFile(string path) {
            var fullPath = Path.Combine(AppContext.BaseDirectory, "Data", path);
            return File.ReadAllText(fullPath);
        }

    }
}
