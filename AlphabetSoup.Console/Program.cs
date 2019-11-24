using AlphabetSoup.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;

namespace AlphabetSoup {
    class Program {
        static void Main(string[] args) {

            var configuration = BuildConfiguration();
            IoC.ConfigureServices(configuration);

            Soup soup = Soup.Build(IoC.Services);
            
            IPrinter printer = IoC.Services.GetService<IPrinter>();
            printer.Options = new PrintOptions {
                PrintAlphabetSoup = true,
                PrintWords = true,
                PrintSolution = true,
                PrintRowNumbers = true
            };
            printer.Print(soup);

            Console.WriteLine();

        }

        private static IConfigurationRoot BuildConfiguration() {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }
    }
}
