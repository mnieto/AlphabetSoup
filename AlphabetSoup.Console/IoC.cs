using AlphabetSoup.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.File;

namespace AlphabetSoup {
    public static class IoC {
        public static IServiceProvider Services { get; private set; }

        public static void ConfigureServices(IConfiguration configuration) {
            IServiceCollection services = new ServiceCollection();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            logger.Information($"Starting application {new AssemblyInfo().GetAssemblyVersion()}");
            services.AddLogging(cfg => cfg.AddSerilog(logger));

            services.AddTransient<IPrinter, Printer>();
            services.AddSingleton<IConsoleWrapper, ConsoleWrapper>();

            services.AddAlphabetSoup(configuration);

            Services = services.BuildServiceProvider();

        }
    }
}
