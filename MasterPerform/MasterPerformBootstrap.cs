using MasterPerform.Entities;
using MasterPerform.Infrastructure.Bootstrap;
using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.ElasticSearch;
using MasterPerform.Infrastructure.Messaging;
using MasterPerform.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MasterPerform
{
    public class MasterPerformBootstrap : MicroserviceBootstrap
    {
        public const string IndexName = "master_perform";

        public MasterPerformBootstrap(IServiceCollection serviceCollection) : base(serviceCollection)
        {
            RegisterIndexInitializer(serviceCollection);
            RegisterCommandHandlers(serviceCollection);
            RegisterRepositories(serviceCollection);
        }

        public static void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterElasticsearchRepositoryFor<Document>();
        }

        private static void RegisterIndexInitializer(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IIndexInitializer, MasterPerformIndexInitializer>();
        }

        protected void RegisterCommandHandlers(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterAllCommandHandlersFromAssemblyContaining<MasterPerformBootstrap>();
        }

        public override void Run(IServiceProvider serviceProvider)
        {
            var indexInitializer = serviceProvider.GetRequiredService<IIndexInitializer>();
            indexInitializer.InitializeIndex();
        }
    }
}
