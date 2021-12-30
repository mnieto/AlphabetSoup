using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AlphabetSoup.Core {
    public static class AlphabetSoupExtensions {
        public static void AddAlphabetSoup(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions();
            services.Configure<Options>(configuration.GetSection("SoupOptions"));
            services.AddTransient<ISoupFactory, SoupFactory>();
            services.AddTransient<ISoupGenerator, SoupGenerator>();
            services.AddTransient<IIntersectionManager, IntersectionManager>();
            services.AddTransient<IBoundariesManager, BoundariesManager>();
        }
    }
}
