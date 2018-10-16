using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup {
    public static class IoC {
        public static IServiceProvider Services { get; private set; }

        public static void ConfigureServices() {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<Printer>();
            services.AddSingleton<IConsoleWrapper, ConsoleWrapper>();

            Services = services.BuildServiceProvider();

        }
    }
}
