using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup {
    public class AssemblyInfo {
        public string GetAssemblyVersion() {
            return GetType().Assembly.GetName().Version.ToString();
        }
    }
}
