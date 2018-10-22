using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AlphabetSoup.Core.Test {
    public class RulesTest {

        [Theory]
        [InlineData(1, 1, Directions.E)]
        [InlineData(5, 1, Directions.E)]
        [InlineData(4, 9, Directions.W)]
        [InlineData(1, 4, Directions.N)]
        public void HaveSpaceToPutNewWord(int x, int y, Directions direction) {
            Soup soup = InitSoup();
            WordEntry entry = new WordEntry {
                X = x,
                Y = y,
                Direction = direction,
                Name = "HELLO"
            };
            IRule rule = new HaveSpace();
            Assert.True(rule.Check(soup, entry));
        }


        [Theory]
        [InlineData(6, 1, Directions.E)]
        [InlineData(3, 9, Directions.W)]
        [InlineData(8, 8, Directions.NE)]
        [InlineData(8, 6, Directions.N)]
        public void DontHaveSpaceToPutNewWord(int x, int y, Directions direction) {
            Soup soup = InitSoup();
            WordEntry entry = new WordEntry {
                X = x,
                Y = y,
                Direction = direction,
                Name = "HELLO"
            };
            IRule rule = new HaveSpace();
            Assert.False(rule.Check(soup, entry));
        }

        [Fact]
        public void NotUsedRule() {
            Soup soup = new Soup {
                UsedWords = new Dictionary<string, WordEntry> {
                    { "Word1", new WordEntry { X = 1, Y = 1, Direction = Directions.E, Name = "Word1"} },
                    { "Word2", new WordEntry { X = 1, Y = 1, Direction = Directions.E, Name = "Word2"} }
                }
            };
            IRule rule = new NotUsed();
            Assert.False(rule.Check(soup, new WordEntry { X = 2, Y = 2, Direction = Directions.W, Name = "Word2" }));
        }

        [Fact]
        public void IsOverlappedRule() {
            SoupGenerator generator = TestDataGenerator.InitGenerator(allowedDirections: Directions.E | Directions.N);
            Soup soup = generator.Init().Create();

            IRule rule = new NotOverlapped();

            //Test that all entries are overlapped
            foreach (WordEntry entry in soup.UsedWords.Values) {
                string testWord = entry.Name.Substring(0, Math.Min(entry.Name.Length, 3));
                Assert.False(rule.Check(soup, new WordEntry { X = entry.X, Y = entry.Y, Direction = entry.Direction, Name = testWord }));
            }

        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.InLengthRangeWords), 4, 6, MemberType = typeof(TestDataGenerator))]
        public void MatchLengthRule(bool expected, string word) {
            IRule rule = new MatchLengthRange(4, 6);
            Assert.Equal(expected, rule.Check(null, new WordEntry { Name = word }));
        }

        /// <summary>
        /// Initialize a small 10x10 test soup with all its cells set to A
        /// </summary>
        private Soup InitSoup() {
            Soup soup = new Soup() {
                Matrix = new char[10, 10]
            };
            for (int x= 0; x < 10; x++) {
                for (int y = 0; y < 10; y++) {
                    soup.Matrix[x, y] = 'A';
                }
            }
            return soup;
        }
    }
}
