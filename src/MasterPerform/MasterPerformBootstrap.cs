using MasterPerform.Contracts.Queries;
using MasterPerform.Descriptors;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Bootstrap;
using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.ElasticSearch;
using MasterPerform.Infrastructure.Messaging;
using MasterPerform.Mapping;
using MasterPerform.QueryBuilders;
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
            RegisterQueryBuilders(serviceCollection);
            RegisterDescriptors(serviceCollection);
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

        private static void RegisterQueryBuilders(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterQueryBuilder<GetDocuments, Document, GetDocumentsQueryBuilder>();
        }
        private static void RegisterDescriptors(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterFullTextSearchDescriptor(MasterPerformFullTextSearchDescriptor.Instance);
        }

        public override void Run(IServiceProvider serviceProvider)
        {
            var indexInitializer = serviceProvider.GetRequiredService<IIndexInitializer>();
            indexInitializer.InitializeIndex();
        }
    }
}
