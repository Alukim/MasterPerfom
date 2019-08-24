using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.WebApi;
using MasterPerform.Tests.Factories;
using MasterPerform.WebApi;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using MasterPerform.Infrastructure.Entities;
using Xunit;

namespace MasterPerform.Tests.Fixtures
{
    public class MasterPerformFixture : IDisposable
    {
        public const string Url = "http://0.0.0.0:5000";
        private readonly TestServer testServer;
        public HttpClient Client { get; }
        public IServiceProvider ServiceProvider { get; }

        public DocumentFactory DocumentFactory { get; }
        public IElasticClient ElasticClient { get; }

        private bool SeedProd = false;
        private string ProdUserName = "--";
        private string ProdPassword = "--";

        public MasterPerformFixture()
        {
            var host = WebHostExtensions.CreatePreconfiguredWebHostBuilder<Startup>()
                .ConfigureAppConfiguration((h, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"EnvironmentSettings:Prefix", $"integration-tests-{DateTime.Now:yyyyMMdd_HHmmss}"}
                    });

                    if (SeedProd)
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string>
                        {
                            {"ElasticsearchSettings:NodeUrl", "http://13.74.64.52:9200"},
                            {"ElasticsearchSettings:Password", ProdPassword},
                            {"ElasticsearchSettings:Username", ProdUserName},
                            {"ElasticsearchSettings:ShardsNumber", "25"}
                        });
                    }
                })
                .ConfigureServices(sc => { sc.AddSingleton<DocumentFactory>(); });

            testServer = new TestServer(host) { BaseAddress = new Uri(Url) };
            Client = testServer.CreateClient();
            ServiceProvider = testServer.Host.Services;

            DocumentFactory = ServiceProvider.GetRequiredService<DocumentFactory>();
            ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();
        }

        public void Dispose()
        {
            RemoveIndexOfType<Document>();
            Client?.Dispose();
            testServer?.Dispose();
        }

        public void RemoveIndexOfType<T>() where T : class, IEntity
        {
            var indexName = ServiceProvider.GetRequiredService<IIndexNameResolver>().GetIndexNameFor<T>();
            ElasticClient.Indices.Delete(indexName);
        }

        public void RefreshIndexOfType<T>() where T : class, IEntity
        {
            var indexName = ServiceProvider.GetRequiredService<IIndexNameResolver>().GetIndexNameFor<T>();
            var response = ElasticClient.Indices.Refresh(indexName);
        }
    }

    [CollectionDefinition(DEFINITION_NAME)]
    public class MasterPerformCollectionFixture : ICollectionFixture<MasterPerformFixture>
    {
        public const string DEFINITION_NAME = "master-perform-integration-tests";
    }
}
