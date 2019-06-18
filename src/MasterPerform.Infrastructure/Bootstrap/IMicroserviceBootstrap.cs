using System;

namespace MasterPerform.Infrastructure.Bootstrap
{
    public interface IMicroserviceBootstrap
    {
        void Run(IServiceProvider serviceProvider);
    }
}
