using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AlphabetSoup.Core.Test {
    public class IntersectionTest {

        [Fact]
        public void IntersectionInfoTest() {
            WordEntry existing = new WordEntry {
                Name = "HOUSE",
                X = 3,
                Y = 2,
                Direction = Directions.N
            };
            WordEntry candidate = new WordEntry {
                Name = "THING",
                X = 2,
                Y = 4,
                Direction = Directions.E
            };
            var sut = new IntersectionManager(existing, candidate);

            Assert.True(sut.Intersects);
            Assert.False(sut.Overlaps);
            Assert.Equal(new Point(3, 4), sut[0]);
        }

        [Fact]
        public void HasCommonLettersTest() {
            WordEntry existing = new WordEntry {
                Name = "HOUSE",
                X = 3,
                Y = 2,
                Direction = Directions.N
            };
            WordEntry candidate = new WordEntry {
                Name = "THING",
                X = 2,
                Y = 4,
                Direction = Directions.E
            };
            var sut = new IntersectionManager(existing, candidate);
            Assert.True(sut.HasCommonLetters());
            Assert.Equal("H", existing.Name.Substring(sut.ExistingRange.Init, sut.ExistingRange.Length));
            Assert.Equal("H", candidate.Name.Substring(sut.CandidateRange.Init, sut.CandidateRange.Length));
        }


        [Fact]
        public void HasCommonLettersIsFalseTest() {
            WordEntry existing = new WordEntry {
                Name = "HOUSE",
                X = 3,
                Y = 2,
                Direction = Directions.N
            };
            WordEntry candidate = new WordEntry {
                Name = "CAT",
                X = 2,
                Y = 4,
                Direction = Directions.E
            };
            var sut = new IntersectionManager(existing, candidate);
            Assert.False(sut.HasCommonLetters());
        }

        [Fact]
        public void TranslateTest() {
            WordEntry existing = new WordEntry {
                Name = "STRANGER",
                X = 8,
                Y = 2,
                Direction = Directions.N
            };
            WordEntry candidate = new WordEntry {
                Name = "THINGS",
                X = 6,
                Y = 4,
                Direction = Directions.E
            };
            var sut = new IntersectionManager(existing, candidate);
            sut.RepositionEntry();
            Assert.Equal(new Point(3, 2), sut.Candidate.Origin);
        }


    }
}
