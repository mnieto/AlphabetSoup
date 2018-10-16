using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphabetSoup {
    public class PrintMultiColumnWords {

        private IConsoleWrapper Console { get; set; }
        private IEnumerable<string> _words;
        private int _colWidth = 0;
        private int? _columns;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="words">List of words</param>
        /// <param name="columns">Number of preferred columns. If <c>null</c> it will compute the <see cref="BestColumnsFit(int)"/></param>
        /// <param name="console"><see cref="System.Console"/> compatible class to output text</param>
        public PrintMultiColumnWords(IConsoleWrapper console, IEnumerable<string> words, int? columns) {
            Console = console;
            _words = words;
            _colWidth = words.Max(x => x.Length) + 1;
            _columns = columns;
        }

        /// <summary>
        /// Gets the number of columns to use
        /// </summary>
        public int Columns => _columns ?? BestColumnsFit(Console.WindowWidth / _colWidth);

        /// <summary>
        /// Computes how best fit the words list in columns
        /// </summary>
        /// <param name="maxColumns">Maximum number of columns. This is the result of divide the media width by the longest word</param>
        /// <returns></returns>
        public int BestColumnsFit(int maxColumns) {
            if (maxColumns == 1)
                return maxColumns;
            int wordCount = _words.Count();
            if (maxColumns >= wordCount)
                return wordCount;

            int i = maxColumns;
            while (i > 1 && wordCount % i != 0) {
                i--;
            }
            return i;
        }

        public void Print() {
            var list = _words.OrderBy(x => x).ToList();
            int i = 0;
            while (i < list.Count) {
                int c = 1;
                StringBuilder sb = new StringBuilder();
                while (i < list.Count && c <= Columns) {
                    sb.Append(list[i].PadRight(_colWidth));
                    c++;
                    i++;
                }
                Console.WriteLine(sb.ToString());
            }
        }
    }
}
