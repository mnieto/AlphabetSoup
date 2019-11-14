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
            StringBuilder sb = new StringBuilder();
            Console.Write(Vertical);
            for (int i = 0; i < width; i++) {
                if (Options.PrintSolution && soup.ShadowMatrix[row, i]) {
                    Console.WriteColor(ConsoleColor.Red, $" {soup.Matrix[row, i]} ");
                } else {
                    Console.Write($" {soup.Matrix[row, i]} ");
                }
                Console.Write(Vertical);
            }
            Console.WriteLine();
        }

        protected string FirstLine(int width) {
            StringBuilder sb = new StringBuilder();
            sb.Append(TopLeft);
            for (int i = 0; i < width; i++) {
                sb.Append(new string(Horizontal, 3));
                sb.Append(TopMiddle);
            }
            sb[sb.Length - 1] = TopRight;
            return sb.ToString();
        }

        protected string MiddleLine(int width) {
            StringBuilder sb = new StringBuilder();
            sb.Append(LeftMiddle);
            for (int i = 0; i < width; i++) {
                sb.Append(new string(Horizontal, 3));
                sb.Append(Cross);
            }
            sb[sb.Length - 1] = RightMiddle;
            return sb.ToString();
        }

        protected string LastLine(int width) {
            StringBuilder sb = new StringBuilder();
            sb.Append(BottomLeft);
            for (int i = 0; i < width; i++) {
                sb.Append(new string(Horizontal, 3));
                sb.Append(BottomMiddle);
            }
            sb[sb.Length - 1] = BottomRight;
            return sb.ToString();
        }

    }
}
