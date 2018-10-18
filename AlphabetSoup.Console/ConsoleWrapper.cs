using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup {
    public class ConsoleWrapper : IConsoleWrapper {
        public int WindowWidth => Console.WindowWidth;

        public void Write(string value) {
            Console.Write(value);
        }

        public void Write(char value) {
            Console.Write(value);
        }

        public void WriteLine() {
            Console.WriteLine();
        }

        public void WriteLine(string value) {
            Console.WriteLine(value);
        }
        public void WriteLine(char value) {
            Console.WriteLine(value);
        }

        public void WriteColor(ConsoleColor color, string value) {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            try {
                Console.Write(value);
            } finally {
                Console.ForegroundColor = previous;
            }
        }

        public void WriteColor(ConsoleColor color, char value) {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            try {
                Console.Write(value);
            } finally {
                Console.ForegroundColor = previous;
            }
        }

        public void WriteColorLine(ConsoleColor color, string value) {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            try {
                Console.WriteLine(value);
            } finally {
                Console.ForegroundColor = previous;
            }

        }

    }
}
