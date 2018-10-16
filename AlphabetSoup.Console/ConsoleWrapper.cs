using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup {
    public class ConsoleWrapper : IConsoleWrapper {
        public int WindowWidth => Console.WindowWidth; 

        public void WriteLine() {
            Console.WriteLine();
        }

        public void WriteLine(string value) {
            Console.WriteLine(value);
        }
    }
}
