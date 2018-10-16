using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup {
    public interface IConsoleWrapper {
        int WindowWidth { get; }
        void WriteLine();
        void WriteLine(string value);
    }
}
