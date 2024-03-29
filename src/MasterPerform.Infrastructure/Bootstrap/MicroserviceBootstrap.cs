﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace MasterPerform.Infrastructure.Bootstrap
{
    public abstract class MicroserviceBootstrap : IMicroserviceBootstrap
    {
        protected readonly IServiceCollection ServiceCollection;

        protected MicroserviceBootstrap(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
        }

        public virtual void Run(IServiceProvider serviceProvider)
        {

        }
    }
}
