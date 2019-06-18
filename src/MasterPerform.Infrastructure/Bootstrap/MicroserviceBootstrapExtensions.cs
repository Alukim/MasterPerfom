using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace MasterPerform.Infrastructure.Bootstrap
{
    public static class MicroserviceBootstrapExtensions
    {
        public static IServiceCollection AddMicroserviceBootstrap<TModuleBootstrap>(this IServiceCollection services)
            where TModuleBootstrap : IMicroserviceBootstrap
        {
            TModuleBootstrap microserviceBootstrap;
            try
            {
                microserviceBootstrap = (TModuleBootstrap)Activator.CreateInstance(typeof(TModuleBootstrap), services);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            return services.AddSingleton<IMicroserviceBootstrap>(microserviceBootstrap);
        }

        public static void UseModule(this IServiceProvider serviceProvider)
        {
            var module = serviceProvider.GetRequiredService<IMicroserviceBootstrap>();

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                module.Run(scope.ServiceProvider);
            }
        }
    }
}
