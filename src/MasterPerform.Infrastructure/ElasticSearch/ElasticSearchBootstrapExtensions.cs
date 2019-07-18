using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using Nest.JsonNetSerializer;
using System;

namespace MasterPerform.Infrastructure.Elasticsearch
{
    public static class ElasticsearchBootstrapExtensions
    {
        public static IServiceCollection AddElasticsearchSettings(this IServiceCollection services, IConfiguration config)
        {
            return services
                .Configure<ElasticsearchSettings>(x => config.GetSection(nameof(ElasticsearchSettings)).Bind(x));
        }

        public static IServiceCollection AddElasticSearchConnection(this IServiceCollection services,
            IConfiguration configuration, string defaultIndexName,
            Func<IServiceProvider, ConnectionSettings, ConnectionSettings> externalConfigurations = null)
        {
            services.AddElasticsearchSettings(configuration);

            services.AddSingleton<IElasticClient>(sp => new ElasticClient(sp.GetService<ConnectionSettings>()));
            services.AddSingleton(sp => new IndexNameResolver(sp.GetRequiredService<ConnectionSettings>()));

            services.AddSingleton(sp =>
            {
                var esSettings = sp.GetService<IOptions<ElasticsearchSettings>>().Value;
                var node = new Uri(esSettings.NodeUrl);
                var pool = new SingleNodeConnectionPool(node);
                var connection = new HttpConnection();

                var connectionSettings = new ConnectionSettings(
                        pool,
                        connection,
                        sourceSerializer: JsonNetSerializer.Default)
                    .DisableDirectStreaming()
                    .PrettyJson()
                    .ThrowExceptions(true)
                    .DefaultIndex(defaultIndexName);

                if (externalConfigurations != null)
                    connectionSettings = externalConfigurations(sp, connectionSettings);

                return connectionSettings;
            });

            return services;
        }
    }
}
