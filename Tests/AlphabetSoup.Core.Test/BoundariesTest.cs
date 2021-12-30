using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AlphabetSoup.Core.Test {
    public class BoundariesTest {

        [Theory]
        [InlineData(8, 8, Directions.E, false)]
        [InlineData(8, 8, Directions.N, false)]
        [InlineData(1, 1, Directions.E, true)]
        [InlineData(6, 6, Directions.NE, true)]
        public void CheckStraightDirection(int x, int y, Directions direction, bool expected) {
            var entry = new WordEntry {
                X = x,
                Y = y,
                Name = "WORD",
                Direction = direction
            };
            var sut = new BoundariesManager(new NullLogger<BoundariesManager>());
            sut.Soup = InitSoup();
            Assert.False(direction.IsReverse());
            Assert.Equal(expected, sut.Check(entry));
        }

        [Theory]
        [InlineData(2, 2, Directions.W, false)]
        [InlineData(2, 2, Directions.S, false)]
        [InlineData(3, 3, Directions.W, true)]
        [InlineData(3, 3, Directions.S, true)]
        public void CheckReverseDirection(int x, int y, Directions direction, bool expected) {
            var entry = new WordEntry {
                X = x,
                Y = y,
                Name = "WORD",
                Direction = direction
            };
            var sut = new BoundariesManager(new NullLogger<BoundariesManager>());
            sut.Soup = InitSoup();
            Assert.True(direction.IsReverse());
            Assert.Equal(expected, sut.Check(entry));
        }


        [Theory]
        [InlineData(8, 8, Directions.E, -2, 0)]
        [InlineData(-1, 8, Directions.E, 1, 0)]
        [InlineData(8, 8, Directions.NE, -2, -2)]
        public void DeltaOnStraightDirection(int x, int y, Directions direction, int dx, int dy) {
            var entry = new WordEntry { 
                X = x,
                Y = y,
                Name = "WORD",
                Direction = direction
            };
            var sut = new BoundariesManager(new NullLogger<BoundariesManager>());
            sut.Soup = InitSoup();
            Point delta = sut.GetDelta(entry);
            Assert.Equal(new Point(dx, dy), delta);
        }

        private Soup InitSoup() {
            Soup soup = new Soup() {
                Matrix = new char[10, 10]
            };
            for (int x = 0; x < 10; x++) {
                for (int y = 0; y < 10; y++) {
                    soup.Matrix[x, y] = 'A';
                }
            }
            return soup;
        }
    }
}
