using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.Infrastructure.EnvironmentPrefixer
{
    public static class BootstrapExtensions
    {
        public static IServiceCollection RegisterEnvironmentSettings(this IServiceCollection serviceCollection,
            IConfiguration config)
            => serviceCollection.Configure<EnvironmentSettings>(x =>
                config.GetSection(nameof(EnvironmentSettings)).Bind(x));

        public static IServiceCollection RegisterEnvironmnentPrefixer(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton<IEnvironmentPrefixer, EnvironmentPrefixer>();

    }
}
