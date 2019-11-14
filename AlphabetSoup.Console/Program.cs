using AlphabetSoup.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AlphabetSoup {
    class Program {
        static void Main(string[] args) {

            IoC.ConfigureServices();

            Soup soup = Soup.Build(new Core.Options {
                CultureCode = "es-es",
                Size = 20,
                NumWords = 10
            });

            
            IPrinter printer = IoC.Services.GetService<IPrinter>();
            printer.Options = new PrintOptions {
                PrintAlphabetSoup = true,
                PrintWords = true,
                PrintSolution = true
            };
            printer.Print(soup);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
