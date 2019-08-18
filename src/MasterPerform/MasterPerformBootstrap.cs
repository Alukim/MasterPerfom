using MasterPerform.Descriptors;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Bootstrap;
using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.ElasticSearch;
using MasterPerform.Infrastructure.Messaging;
using MasterPerform.Mapping;
using MasterPerform.Services;
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
            RegisterDescriptors(serviceCollection);
            RegisterServices(serviceCollection);
        }

        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<FindSimilarService<Document>>();
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
            serviceCollection.RegisterAllQueryHandlersFromAssemblyContaining<MasterPerformBootstrap>();
        }

        private static void RegisterDescriptors(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterFullTextSearchDescriptor(MasterPerformFullTextSearchDescriptor.Instance);
            serviceCollection.RegisterFindSimilarDescriptor(
                MasterPerformFindSimilarDescriptor.Instance);
        }

        public override void Run(IServiceProvider serviceProvider)
        {
            var indexInitializer = serviceProvider.GetRequiredService<IIndexInitializer>();
            indexInitializer.InitializeIndex();
        }
    }
}
