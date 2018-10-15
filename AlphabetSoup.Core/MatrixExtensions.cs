using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core {
    public static class MatrixExtensions {

        /// <summary>
        /// Gets an array corresponding to the specified <paramref name="row"/> from the matrix
        /// </summary>
        /// <typeparam name="T">data type of the matrix</typeparam>
        /// <param name="matrix">matrix</param>
        /// <param name="row">row number to get the array</param>
        /// <returns></returns>
        public static T[] GetRow<T>(this T[,] matrix, int row) {
            var rowLength = matrix.GetLength(0);
            var rowVector = new T[rowLength];

            for (var i = 0; i < rowLength; i++)
                rowVector[i] = matrix[row, i];

            return rowVector;
        }
    }
}
