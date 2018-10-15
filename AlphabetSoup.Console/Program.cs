using AlphabetSoup.Core;
using System;

namespace AlphabetSoup {
    class Program {
        static void Main(string[] args) {

            Soup soup = Soup.Build(new Core.Options {
                CultureCode = "es-es",
                Size = 20,
                NumWords = 6
            });

            Printer printer = new Printer();
            printer.Print(soup);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
