﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AlphabetSoup.Core.Test {
    public class SoupTest {

        [Theory]
        [InlineData(Directions.N, Directions.N)]
        [InlineData(Directions.N, Directions.S)]
        [InlineData(Directions.NE, Directions.SW)]
        [InlineData(Directions.NW, Directions.SE)]
        public void EquivalentDirectionsTest(Directions dir1, Directions dir2) {
            Assert.True(dir1.SameDirection(dir2));
        }


        [Theory]
        [InlineData(6, 2, 6, 2, Directions.E)]
        [InlineData(5, 3, 9, 3, Directions.W)]
        [InlineData(5, 3, 9, 7, Directions.SW)]
        public void AbsoluteOriginTest(int expectedX, int expectedY, int x, int y, Directions direction) {
            WordEntry entry = new WordEntry {
                X = x,
                Y = y,
                Direction = direction,
                Name = "HELLO"
            };
            Assert.Equal(new Point(expectedX, expectedY), entry.AbsoluteOrigin());
        }

        [Theory]
        [InlineData(Directions.NW)]
        [InlineData(Directions.S)]
        [InlineData(Directions.SW)]
        [InlineData(Directions.W)]
        public void IsReverseTest(Directions dir) {
            Assert.True(dir.IsReverse());
        }

        [Fact]
        public void MatrixDimensions() {
            Soup soup = new Soup {
                Matrix = new char[10, 5]
            };

            Assert.Equal(10, soup.Matrix.GetLength(0)); //The X axis
            Assert.Equal(5, soup.Matrix.GetLength(1));  //The Y axis
        }
    }
}
