using AlphabetSoup.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphabetSoup {

    /// <summary>
    /// Prints out the alphabet soup to Console
    /// </summary>
    public class Printer {

        private const char TopLeft = '┌';
        private const char Horizontal = '─';
        private const char TopMiddle = '┬';
        private const char TopRight = '┐';
        private const char LeftMiddle = '├';
        private const char Cross = '┼';
        private const char RightMiddle = '┤';
        private const char Vertical = '│';
        private const char BottomLeft = '└';
        private const char BottomMiddle = '┴';
        private const char BottomRight = '┘';


        private PrintOptions Options { get; set; }

        /// <summary>
        /// Default constructor. Creates de printer object with default <see cref="PrintOptions"/> 
        /// </summary>
        public Printer() : this(new PrintOptions()) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">What to print</param>
        public Printer(PrintOptions options) {
            Options = options;
        }

        public void Print(Soup soup) {
            if (soup == null) {
                throw new InvalidOperationException($"{nameof(soup)} can't be null");
            }

            if (Options.PrintAlphabetSoup) {
                PrintSoup(soup);
            }
            if (Options.PrintWords) {
                Console.WriteLine();
                PrintWords(soup.UsedWords.Keys);
            }
        }

        protected void PrintSoup(Soup soup) {
            int width = soup.Matrix.GetLength(0);
            int height = soup.Matrix.GetLength(1);

            Console.WriteLine(FirstLine(width));
            for (int i = 0; i < height; i++) {
                Console.WriteLine(DataLine(soup.Matrix.GetRow(i)));
                if (i < height - 1) {
                    Console.WriteLine(MiddleLine(width));
                }
            }
            Console.WriteLine(LastLine(width));
        }

        protected void PrintWords(IEnumerable<string> words) {
            var wordPrinter = new PrintMultiColumnWords(words, Options.WordColumns);
            wordPrinter.Print();
        }

        protected string DataLine(char[] row) {
            int width = row.Length;
            StringBuilder sb = new StringBuilder();
            sb.Append(Vertical);
            for (int i = 0; i < width; i++) {
                //TODO: Write in other color if PrintOptions.Solution is set
                sb.Append(row[i]);
                sb.Append(Vertical);
            }
            return sb.ToString();
        }
        protected string FirstLine(int width) {
            StringBuilder sb = new StringBuilder();
            sb.Append(TopLeft);
            for (int i = 0; i < width; i++) {
                sb.Append(Horizontal);
                sb.Append(TopMiddle);
            }
            sb[sb.Length - 1] = TopRight;
            return sb.ToString();
        }

        protected string MiddleLine(int width) {
            StringBuilder sb = new StringBuilder();
            sb.Append(LeftMiddle);
            for (int i = 0; i < width; i++) {
                sb.Append(Horizontal);
                sb.Append(Cross);
            }
            sb[sb.Length - 1] = RightMiddle;
            return sb.ToString();
        }

        protected string LastLine(int width) {
            StringBuilder sb = new StringBuilder();
            sb.Append(BottomLeft);
            for (int i = 0; i < width; i++) {
                sb.Append(Horizontal);
                sb.Append(BottomMiddle);
            }
            sb[sb.Length - 1] = BottomRight;
            return sb.ToString();
        }

    }
}
