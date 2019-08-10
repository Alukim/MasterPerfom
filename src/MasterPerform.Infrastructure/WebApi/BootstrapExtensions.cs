using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.Infrastructure.WebApi
{
    public static class BootstrapExtensions
    {
        public static IServiceCollection AddContextAccessors(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            return serviceCollection;
        }
    }
}
