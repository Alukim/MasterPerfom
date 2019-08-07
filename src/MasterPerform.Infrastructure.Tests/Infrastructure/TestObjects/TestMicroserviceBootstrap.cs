using MasterPerform.Infrastructure.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.Tests.Infrastructure.TestObjects
{
    internal class TestMicroserviceBootstrap : MicroserviceBootstrap
    {
        public TestMicroserviceBootstrap(IServiceCollection serviceCollection) : base(serviceCollection)
        {
        }
    }
}
