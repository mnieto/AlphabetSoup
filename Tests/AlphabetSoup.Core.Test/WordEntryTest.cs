using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AlphabetSoup.Core.Test {
    public class WordEntryTest {

        [Theory]
        [InlineData(9, 9, Directions.E)]
        [InlineData(9, 5, Directions.S)]
        [InlineData(5, 13, Directions.NW)]
        [InlineData(5, 5, Directions.SW)]
        [InlineData(5, 9, Directions.W)]
        void AbsoluteOriginTest(int x, int y, Directions direction) {
            var sut = new WordEntry {
                X = 9,
                Y = 9,
                Name = "HELLO",
                Direction = direction
            };
            Assert.Equal(new Point(x, y), sut.AbsoluteOrigin());

        }

        [Theory]
        [ClassData(typeof(CoordinatesData))]
        public void CoordinateTest(Directions direction, List<Point> points) {
            var sut = new WordEntry {
                X = 9,
                Y = 9,
                Name = "HELLO",
                Direction = direction
            };
            var positions = new List<Point>(sut.Name.Length);
            for (int i = 0; i < sut.Name.Length; i++) {
                positions.Add(sut.Coordinate(i));
            }

            var common = positions.Intersect(points);
            Assert.Equal(positions.Count, common.Count());
        }

        [Fact]
        void ToStringTetst() {
            var sut = new WordEntry {
                X = 1,
                Y = 1,
                Name = "HELLO",
                Direction = Directions.E
            };

            Assert.Equal("HELLO at (1, 1) with E", sut.ToString());
        }



            
    }

    public class CoordinatesData : IEnumerable<object[]> {

        int length;
        public CoordinatesData() {
            length = "HELLO".Length;
        }

        public IEnumerator<object[]> GetEnumerator() {
            var origin = new Point(9, 9);
            return new List<object[]> {
                new object[] { Directions.E,  GetPoints(origin,  1,  0) },
                new object[] { Directions.N,  GetPoints(origin,  0,  1) },
                new object[] { Directions.NE, GetPoints(origin,  1,  1) },
                new object[] { Directions.NW, GetPoints(origin, -1,  1) },
                new object[] { Directions.S,  GetPoints(origin,  0, -1) },
                new object[] { Directions.SE, GetPoints(origin,  1, -1) },
                new object[] { Directions.SW, GetPoints(origin, -1, -1) },
                new object[] { Directions.W,  GetPoints(origin, -1,  0) }
            }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        private List<Point> GetPoints(Point origin, int deltaX, int deltaY) {
            var result = new List<Point>();
            int x = origin.X;
            int y = origin.Y;
            for (int i = 0; i < length; i++) {
                result.Add(new Point(x, y));
                x += deltaX;
                y += deltaY;
            }
            return result;
        }


    }
}
