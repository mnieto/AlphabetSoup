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
