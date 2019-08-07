using MasterPerform.Infrastructure.WebApi;
using MasterPerform.WebApi;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace MasterPerform.Tests.Fixtures
{
    public class MasterPerformFixture : IDisposable
    {
        public const string Url = "http://0.0.0.0:5000";
        private readonly TestServer testServer;
        public HttpClient Client { get; }
        public IServiceProvider ServiceProvider { get; }

        public MasterPerformFixture()
        {
            var host = WebHostExtensions.CreatePreconfiguredWebHostBuilder<Startup>()
                .ConfigureAppConfiguration((h, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"EnvironmentSettings:Prefix", $"integration-tests-{DateTime.Now.Date.ToShortDateString()}"}
                    });
                });

            testServer = new TestServer(host) { BaseAddress = new Uri(Url) };
            Client = testServer.CreateClient();
            ServiceProvider = testServer.Host.Services;
        }

        public void Dispose()
        {
            Client?.Dispose();
            testServer?.Dispose();
        }
    }

    [CollectionDefinition(DEFINITION_NAME)]
    public class MasterPerformCollectionFixture : ICollectionFixture<MasterPerformFixture>
    {
        public const string DEFINITION_NAME = "master-perform-integration-tests";
    }
}
