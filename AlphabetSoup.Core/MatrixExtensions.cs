using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {
    public static class MatrixExtensions {
        public static T[] GetRow<T>(this T[,] matrix, int row) {
            var rowLength = matrix.GetLength(1);
            var rowVector = new T[rowLength];

            for (var i = 0; i < rowLength; i++)
                rowVector[i] = matrix[row, i];

            return rowVector;
        }
    }
}
