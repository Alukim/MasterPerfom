using Elasticsearch.Net;
using MasterPerform.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using Nest.JsonNetSerializer;
using System;

namespace MasterPerform.Infrastructure.ElasticSearch
{
    public static class ElasticSearchBootstrapExtensions
    {
        public static IServiceCollection AddIndexName<TIndex>(this IServiceCollection serviceCollection, string indexName)
            where TIndex : IEntity
        {
            return serviceCollection
                .AddSingleton(x => new IndexNameProvider<TIndex>(indexName));
        }

        public static IServiceCollection AddElasticsearchSettings(this IServiceCollection services, IConfiguration config)
        {
            return services
                .Configure<ElasticSearchSettings>(config.GetSection(nameof(ElasticSearchSettings)));
        }

        public static IServiceCollection AddElasticSearchConnection(this IServiceCollection services,
            IConfiguration configuration,
            Func<IServiceProvider, ConnectionSettings, ConnectionSettings> externalConfigurations = null)
        {

            services.AddElasticsearchSettings(configuration);

            services.AddSingleton<IElasticClient>(sp => new ElasticClient(sp.GetService<ConnectionSettings>()));

            services.AddSingleton(sp =>
            {
                var esSettings = sp.GetService<IOptions<ElasticSearchSettings>>().Value;
                var node = new Uri(esSettings.NodeUrl);
                var pool = new SingleNodeConnectionPool(node);
                var connection = new HttpConnection();

                var connectionSettings = new ConnectionSettings(
                        pool,
                        connection,
                        JsonNetSerializer.Default)
                    .DisableDirectStreaming()
                    .PrettyJson()
                    .ThrowExceptions(true);

                if (externalConfigurations != null)
                    connectionSettings = externalConfigurations(sp, connectionSettings);

                return connectionSettings;
            });

            return services;
        }
    }
}
