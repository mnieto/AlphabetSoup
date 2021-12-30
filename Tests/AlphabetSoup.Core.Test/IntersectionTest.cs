using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AlphabetSoup.Core.Test {
    public class IntersectionTest {
        
        private Soup soup;
        public IntersectionTest() {
            var dataGenerator = IoC.Services.GetService<TestDataGenerator>();
            SoupGenerator generator = dataGenerator.InitGenerator(allowedDirections: Directions.E | Directions.N);
            soup = generator.Init().Soup;
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
            var sut = SetupIntersectionManager(existing, candidate);

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
            var sut = SetupIntersectionManager(existing, candidate);
            
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
            var sut = SetupIntersectionManager(existing, candidate);
            
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
            var sut = SetupIntersectionManager(existing, candidate);
            Assert.True(sut.GetCommonLetters());
            Assert.Equal("LY", existing.Name.Substring(sut.ExistingRange.Init, sut.ExistingRange.Length));
            Assert.Equal("LY", candidate.Name.Substring(sut.CandidateRange.Init, sut.CandidateRange.Length));

            //The found range must match, in order and quantity, with CommonLetters
            Assert.Equal(existing.Name.Substring(sut.ExistingRange.Init, sut.ExistingRange.Length).ToCharArray(), sut.CommonLetters.Select(x => x.Letter));
            Assert.Equal(candidate.Name.Substring(sut.CandidateRange.Init, sut.CandidateRange.Length).ToCharArray(), sut.CommonLetters.Select(x => x.Letter));

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
            var sut = SetupIntersectionManager(existing, candidate);
            Assert.True(sut.GetCommonLetters());
            Assert.Equal("ER", existing.Name.Substring(sut.ExistingRange.Init, sut.ExistingRange.Length));
            Assert.Equal("RE", candidate.Name.Substring(sut.CandidateRange.Init, sut.CandidateRange.Length));

            //The found range must mutch, in order and quantity, with CommonLetters
            Assert.Equal(existing.Name.Substring(sut.ExistingRange.Init, sut.ExistingRange.Length).ToCharArray(), sut.CommonLetters.Select(x => x.Letter));
            Assert.Equal(candidate.Name.Substring(sut.CandidateRange.Init, sut.CandidateRange.Length).ToCharArray(), sut.CommonLetters.Select(x => x.Letter).Reverse());

        }

        [Fact]
        public void GetCommonLettersAngleDirection() {
            WordEntry existing = new WordEntry {
                Name = "RETOBADO",
                X = 8,
                Y = 3,
                Direction = Directions.NE
            };
            WordEntry candidate = new WordEntry {
                Name = "CAMBIZO",
                X = 9,
                Y = 4,
                Direction = Directions.E
            };
            var sut = SetupIntersectionManager(existing, candidate);
            Assert.True(sut.GetCommonLetters());
            Assert.Collection(sut.CommonLetters,
                x => Assert.Equal(new IntersectionManager.CommonLetter { Letter = 'O', ExistingPos = 3, CandidatePos = 6 }, x),
                x => Assert.Equal(new IntersectionManager.CommonLetter { Letter = 'B', ExistingPos = 4, CandidatePos = 3 }, x),
                x => Assert.Equal(new IntersectionManager.CommonLetter { Letter = 'A', ExistingPos = 5, CandidatePos = 1 }, x),
                x => Assert.Equal(new IntersectionManager.CommonLetter { Letter = 'O', ExistingPos = 7, CandidatePos = 6 }, x)
            );

        }

        [Fact]
        public void TranslateWithIntersectionTest() {
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
            var sut = SetupIntersectionManager(existing, candidate);
            candidate = sut.RepositionEntry();
            Assert.Equal(new Point(3, 2), candidate.Origin);
        }

        [Fact]
        public void TranslateWithSameDirectionTest() {
            WordEntry existing = new WordEntry {
                Name = "JULY",
                X = 3,
                Y = 2,
                Direction = Directions.E
            };
            WordEntry candidate = new WordEntry {
                Name = "LYRICS",
                X = 4,
                Y = 2,
                Direction = Directions.E
            };
            var sut = SetupIntersectionManager(existing, candidate);
            candidate = sut.RepositionEntry();
            Assert.Equal(new Point(5, 2), candidate.Origin);
        }

        [Fact]
        public void TranslateWithOppositeDirectionTest() {
            WordEntry existing = new WordEntry {
                Name = "WINTER",
                X = 3,
                Y = 4,
                Direction = Directions.E
            };
            WordEntry candidate = new WordEntry {
                Name = "WHORE",
                X = 4,
                Y = 4,
                Direction = Directions.W
            };
            var sut = SetupIntersectionManager(existing, candidate);
            candidate = sut.RepositionEntry();
            Assert.Equal(new Point(11, 4), candidate.Origin);
        }

        [Fact]
        public void RepositionEntry() {
            WordEntry existing = new WordEntry {
                Name = "SAGRARIO",
                X = 2,
                Y = 3,
                Direction = Directions.NE
            };
            WordEntry candidate = new WordEntry {
                Name = "BUSCADA",
                X = 8,
                Y = 4,
                Direction = Directions.N
            };
            var sut = SetupIntersectionManager(existing, candidate);
            candidate = sut.RepositionEntry();
            Assert.Equal(new Point(2, 1), candidate.Origin);
        }

        [Fact]
        public void GetIntersectionAndReposition() {
            WordEntry existing = new WordEntry {
                Name = "RETOBADO",
                X = 8,
                Y = 3,
                Direction = Directions.NE
            };
            WordEntry candidate = new WordEntry {
                Name = "CAMBIZO",
                X = 9,
                Y = 4,
                Direction = Directions.E
            };
            var sut = SetupIntersectionManager(existing, candidate);
            sut.GetIntersection();
            Assert.Collection(sut, x => Assert.Equal(new Point(9, 4), x));
            candidate = sut.RepositionEntry();
            Assert.Equal(new Point(5, 6), candidate.Origin);

        }

        private IntersectionManager SetupIntersectionManager(WordEntry existing, WordEntry candidate) {
            var sut = new IntersectionManager(NullLogger<IntersectionManager>.Instance);
            sut.Soup = soup;
            sut.Check(existing, candidate);
            return sut;
        }

    }
}
