using AlphabetSoup.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AlphabetSoup.Core.Test {
    public class SoupGeneratorTest {

        [Fact]
        public void MembersAreInitializedAfterConstructorInvoke() {
            var sut = InitGenerator();
            Assert.Equal(TestDataGenerator.Words.Count, sut.Words.Count);
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
            Assert.False(sut.Soup.ShadowMatrix[0, 0]);
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

            Assert.Equal(3, generator.Rules.Count());
        }

        private SoupGenerator InitGenerator() {
            return TestDataGenerator.InitGenerator();
        }

        protected string ReadTestFile(string path) {
            var fullPath = Path.Combine(AppContext.BaseDirectory, "Data", path);
            return File.ReadAllText(fullPath);
        }

    }
}
