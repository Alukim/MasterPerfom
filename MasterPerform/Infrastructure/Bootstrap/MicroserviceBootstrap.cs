using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace MasterPerform.Infrastructure.Bootstrap
{
    public static class MicroserviceBootstrapExtensions
    {
        public static void AddMicroserviceBootstrap<TModuleBootstrap>(this IServiceCollection services)
            where TModuleBootstrap : MicroserviceBootstrap
        {
            MicroserviceBootstrap microserviceBootstrap;
            try
            {
                microserviceBootstrap = (MicroserviceBootstrap)Activator.CreateInstance(typeof(TModuleBootstrap), services);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            services.AddSingleton(microserviceBootstrap);
        }

        public static void UseModule(this IServiceProvider serviceProvider)
        {
            var module = serviceProvider.GetRequiredService<MicroserviceBootstrap>();

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                module.Run(scope.ServiceProvider);
            }
        }
    }


    public abstract class MicroserviceBootstrap
    {
        protected readonly IServiceCollection container;

        protected MicroserviceBootstrap(IServiceCollection container)
            => this.container = container;

        public virtual void Run(IServiceProvider serviceProvider)
        {

        }
    }
}
