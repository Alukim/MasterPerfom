using Elasticsearch.Net;
using MasterPerform.Infrastructure.Elasticsearch.Descriptors;
using MasterPerform.Infrastructure.Elasticsearch.Queries;
using MasterPerform.Infrastructure.Entities;
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

        public static IServiceCollection RegisterIndexNameResolver(this IServiceCollection services)
        {
            return services.AddSingleton<IIndexNameResolver, EnvironmentIndexNameResolver>();
        }

        public static IServiceCollection AddElasticSearchConnection(this IServiceCollection services,
            IConfiguration configuration, string defaultIndexName,
            Func<IServiceProvider, ConnectionSettings, ConnectionSettings> externalConfigurations = null)
        {
            services.AddElasticsearchSettings(configuration);
            services.RegisterIndexNameResolver();

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

                if (!string.IsNullOrEmpty(esSettings.Username) && !string.IsNullOrEmpty(esSettings.Password))
                {
                    connectionSettings = connectionSettings
                        .BasicAuthentication(esSettings.Username, esSettings.Password);
                }

                if (externalConfigurations != null)
                    connectionSettings = externalConfigurations(sp, connectionSettings);

                return connectionSettings;
            });

            return services;
        }

        public static IServiceCollection RegisterQueryBuilder<TQuery, TEntity, TBuilder>(
            this IServiceCollection serviceCollection)
            where TQuery : class
            where TEntity : class, IEntity
            where TBuilder : class, IElasticsearchQueryBuilder<TQuery, TEntity>
            => serviceCollection.AddScoped<IElasticsearchQueryBuilder<TQuery, TEntity>, TBuilder>();

        public static IServiceCollection RegisterFullTextSearchDescriptor<TIndex>(
            this IServiceCollection serviceCollection, IFullTextSearchDescriptor<TIndex> instance)
            where TIndex : class, IEntity
            => serviceCollection.AddSingleton(instance);

        public static IServiceCollection RegisterFindSimilarDescriptor<TIndex>(
            this IServiceCollection serviceCollection, IFindSimilarDescriptor<TIndex> instance)
            where TIndex : class, IEntity
            => serviceCollection.AddSingleton(instance);
    }
}
