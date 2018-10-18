using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup {
    public interface IConsoleWrapper {
        int WindowWidth { get; }
        void Write(string value);
        void Write(char value);
        void WriteLine();
        void WriteLine(string value);
        void WriteLine(char value);
        void WriteColor(ConsoleColor color, string value);
        void WriteColorLine(ConsoleColor color, string value);
        void WriteColor(ConsoleColor color, char value);

    }
}