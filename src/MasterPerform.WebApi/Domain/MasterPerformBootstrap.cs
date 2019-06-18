using MasterPerform.Infrastructure.Bootstrap;
using MasterPerform.Infrastructure.ElasticSearch;
using MasterPerform.Infrastructure.Messaging;
using MasterPerform.WebApi.Contracts;
using MasterPerform.WebApi.Domain.Handlers;
using MasterPerform.WebApi.Domain.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MasterPerform.WebApi.Domain
{
    public class MasterPerformBootstrap : MicroserviceBootstrap
    {
        public const string IndexName = "master_perform";

        public MasterPerformBootstrap(IServiceCollection serviceCollection) : base(serviceCollection)
        {
            RegisterIndexInitializer(serviceCollection);
        }

        private static void RegisterIndexInitializer(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IIndexInitializer, MasterPerformIndexInitializer>();
        }

        protected override void RegisterCommandHandlers(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterCommandHandler<CreateDocument, CreateDocumentCommandHandler>();
        }

        public override void Run(IServiceProvider serviceProvider)
        {
            var indexInitializer = serviceProvider.GetRequiredService<IIndexInitializer>();
            indexInitializer.InitializeIndex();
        }
    }
}
