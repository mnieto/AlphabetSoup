using AlphabetSoup.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphabetSoup {


    /// <summary>
    /// Prints out the alphabet soup to Console
    /// </summary>
    public class Printer : IPrinter {

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
        private const int RowNumberWidth = 2;

        private IConsoleWrapper Console { get; set; }

        public PrintOptions Options { get; set; }

        /// <summary>
        /// Default constructor. Creates de printer object with default <see cref="PrintOptions"/> 
        /// </summary>
        /// <param name="console"><see cref="System.Console"/> compatible class to output text</param>
        public Printer(IConsoleWrapper console) : this(console, new PrintOptions()) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="console"><see cref="System.Console"/> compatible class to output text</param>
        /// <param name="options">What to print</param>
        public Printer(IConsoleWrapper console, PrintOptions options) {
            Console = console;
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

        public void PrintSoup(Soup soup) {
            int width = soup.Matrix.GetLength(0);
            int height = soup.Matrix.GetLength(1);

            if (Options.PrintRowNumbers) {
                Console.WriteLine(ColumnHeaders(width));
            }
            Console.WriteLine(FirstLine(width));
            for (int i = 0; i < height; i++) {
                WriteDataLine(soup, i);
                if (i < height - 1) {
                    Console.WriteLine(MiddleLine(width));
                }
            }
            Console.WriteLine(LastLine(width));
        }

        public void PrintWords(IEnumerable<string> words) {
            var wordPrinter = new PrintMultiColumnWords(Console, words, Options.WordColumns);
            wordPrinter.Print();
        }

        protected void WriteDataLine(Soup soup, int row) {
            int width = soup.Matrix.GetLength(0);
            if (Options.PrintRowNumbers) {
                Console.Write(RowLabel(row));
            }
            Console.Write(Vertical);
            for (int x = 0; x < width; x++) {
                if (Options.PrintSolution && soup.ShadowMatrix[x, row]) {
                    Console.WriteColor(ConsoleColor.Blue, $" {soup.Matrix[x, row]} ");
                } else {
                    Console.Write($" {soup.Matrix[x, row]} ");
                }
                Console.Write(Vertical);
            }
            Console.WriteLine();
        }

        private string RowLabel(int label) {
            string asString = label.ToString();
            if (asString.Length > RowNumberWidth)
                asString = asString.Substring(asString.Length - RowNumberWidth, RowNumberWidth);
            return $"{asString,-RowNumberWidth}";
        }

        protected string ColumnHeaders(int width) {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(' ', RowNumberWidth));
            for (int i = 0; i < width; i++) {
                sb.Append("  ");
                sb.Append(RowLabel(i));
            }
            return sb.ToString();
        }

        protected string FirstLine(int width) {
            return Line(width, TopLeft, Horizontal, TopMiddle, TopRight);
        }

        protected string MiddleLine(int width) {
            return Line(width, LeftMiddle, Horizontal, Cross, RightMiddle);
        }

        protected string LastLine(int width) {
            return Line(width, BottomLeft, Horizontal, BottomMiddle, BottomRight);
        }

        protected string Line(int width, char left, char midle, char cross, char right) {
            StringBuilder sb = new StringBuilder();
            sb.Append(Options.PrintRowNumbers ? new string(' ', RowNumberWidth) : "");
            sb.Append(left);
            for (int i = 0; i < width; i++) {
                sb.Append(new string(midle, 3));
                sb.Append(cross);
            }
            sb[sb.Length - 1] = right;
            return sb.ToString();
        }

    }
}
