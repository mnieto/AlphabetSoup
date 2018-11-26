using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AlphabetSoup.Core.Test {
    public class IntersectionTest {
        
        private Soup soup;
        public IntersectionTest() {
            soup = TestDataGenerator.InitGenerator().Init().Soup;
        }

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
            var sut = new IntersectionManager(existing, candidate, soup);

            Assert.True(sut.Intersects);
            Assert.False(sut.Overlaps);
            Assert.Equal(new Point(3, 4), sut[0]);
        }

        [Fact]
        public void HasCommonLettersTest() {
            WordEntry existing = new WordEntry {
                Name = "STRANGER",
                X = 3,
                Y = 2,
                Direction = Directions.N
            };
            WordEntry candidate = new WordEntry {
                Name = "THINGS",
                X = 2,
                Y = 4,
                Direction = Directions.E
            };
            var sut = new IntersectionManager(existing, candidate, soup);
            Assert.True(sut.GetCommonLetters());
            Assert.Equal(4, sut.CommonLetters.Count);
            Assert.Equal('S', sut.CommonLetters[0].Letter);
            Assert.Equal(0, sut.CommonLetters[0].ExistingPos);
            Assert.Equal(5, sut.CommonLetters[0].CandidatePos);
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
            var sut = new IntersectionManager(existing, candidate, soup);
            Assert.False(sut.GetCommonLetters());
        }

        [Fact]
        public void HasCommonLettersInSameDirection() {
            WordEntry existing = new WordEntry {
                Name = "JULY",
                X = 3,
                Y = 2,
                Direction = Directions.E
            };
            WordEntry candidate = new WordEntry {
                Name = "LYRICS",
                X = 2,
                Y = 4,
                Direction = Directions.E
            };
            var sut = new IntersectionManager(existing, candidate, soup);
            Assert.True(sut.GetCommonLetters());
            Assert.Equal("LY", existing.Name.Substring(sut.ExistingRange.Init, sut.ExistingRange.Length));
            Assert.Equal("LY", candidate.Name.Substring(sut.CandidateRange.Init, sut.CandidateRange.Length));
        }

        [Fact]
        public void HasCommonLettersInOpositeDirection() {
            WordEntry existing = new WordEntry {
                Name = "WINTER",
                X = 3,
                Y = 2,
                Direction = Directions.E
            };
            WordEntry candidate = new WordEntry {
                Name = "WIRE",
                X = 3,
                Y = 4,
                Direction = Directions.W
            };
            var sut = new IntersectionManager(existing, candidate, soup);
            Assert.True(sut.GetCommonLetters());
            Assert.Equal("ER", existing.Name.Substring(sut.ExistingRange.Init, sut.ExistingRange.Length));
            Assert.Equal("RE", candidate.Name.Substring(sut.CandidateRange.Init, sut.CandidateRange.Length));

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
            var sut = new IntersectionManager(existing, candidate, soup);
            sut.RepositionEntry();
            Assert.Equal(new Point(3, 2), sut.Candidate.Origin);
        }


    }
}
