using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphabetSoup.Core.Test {
    static class IoC {

        public static IServiceProvider Services = ConfigureServices();

        private static IServiceProvider ConfigureServices() {
            IServiceCollection services = new ServiceCollection();

            services.AddOptions();
            services.AddTransient(typeof(ILogger<BoundariesManager>), typeof(NullLogger<BoundariesManager>));
            services.AddTransient(typeof(ILogger<IntersectionManager>), typeof(NullLogger<IntersectionManager>));
            services.AddTransient<ISoupFactory, SoupFactory>();
            services.AddTransient<ISoupGenerator, SoupGenerator>();
            services.AddTransient<IBoundariesManager, BoundariesManager>();
            services.AddTransient<IIntersectionManager, IntersectionManager>();
            services.AddTransient<TestDataGenerator>();

            return services.BuildServiceProvider();
        }
    }
}
