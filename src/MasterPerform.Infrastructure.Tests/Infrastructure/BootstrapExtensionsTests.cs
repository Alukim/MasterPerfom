using FluentAssertions;
using MasterPerform.Infrastructure.Bootstrap;
using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.EnvironmentPrefixer;
using MasterPerform.Infrastructure.Messaging;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Tests.Infrastructure.TestObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using Xunit;
using ConfigurationSettings = MasterPerform.Tests.Infrastructure.TestObjects.ConfigurationSettings;

namespace MasterPerform.Tests.Infrastructure
{
    [Trait(CollectionName, CollectionDescription)]
    public class BootstrapExtensionsTests
    {
        private const string CollectionName = "Bootstrap extensions";
        private const string CollectionDescription = "Bootstrap extensions tests.";

        [Fact(DisplayName = "AddMicroserviceBootstrap should add proper implementation.")]
        public void AddMicroserviceBootstrap_ShouldAddProperImplementation()
        {
            new ServiceCollection()
                .AddMicroserviceBootstrap<TestMicroserviceBootstrap>()
                .BuildServiceProvider()
                .GetRequiredService<IMicroserviceBootstrap>()
                .GetType()
                .Should()
                .Be(typeof(TestMicroserviceBootstrap));
        }

        [Fact(DisplayName = "AddElasticsearchSettings should add proper configuration.")]
        public void AddElasticsearchSettings_ShouldAddProperConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(ConfigurationSettings.CustomSettings)
                .Build();

            var elasticsearchSettings = new ServiceCollection()
                .AddElasticsearchSettings(config)
                .BuildServiceProvider()
                .GetRequiredService<IOptions<ElasticsearchSettings>>()
                .Value;

            elasticsearchSettings.Should().NotBeNull();
            elasticsearchSettings.NodeUrl.Should().Be(ConfigurationSettings.NodeUrl);
            elasticsearchSettings.ShardsNumber.Should().Be(ConfigurationSettings.ShardsNumber);
        }

        [Fact(DisplayName = "AddElasticSearchConnection should add proper configuration.")]
        public void AddElasticSearchConnection_ShouldAddProperConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(ConfigurationSettings.CustomSettings)
                .Build();

            var indexName = "TEST";

            var serviceProvider = new ServiceCollection()
                .AddElasticSearchConnection(config, indexName)
                .RegisterEnvironmnentPrefixer()
                .RegisterEnvironmentSettings(config)
                .BuildServiceProvider();

            var elasticsearchSettings = serviceProvider.GetRequiredService<IOptions<ElasticsearchSettings>>().Value;
            var indexNameResolver = serviceProvider.GetRequiredService<IIndexNameResolver>();
            var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();

            elasticsearchSettings.Should().NotBeNull();
            elasticsearchSettings.NodeUrl.Should().Be(ConfigurationSettings.NodeUrl);
            elasticsearchSettings.ShardsNumber.Should().Be(ConfigurationSettings.ShardsNumber);

            indexNameResolver.GetType().Should().Be(typeof(EnvironmentIndexNameResolver));
            indexNameResolver.GetIndexNameFor<TestEntity>().Should().Be($"{ConfigurationSettings.Prefix}-{indexName}");

            elasticClient.GetType().Should().Be(typeof(ElasticClient));
        }

        [Fact(DisplayName = "RegisterCommandQueryProvider should add proper implementation.")]
        public void RegisterCommandQueryProvider_ShouldAddProperImplementation()
        {
            new ServiceCollection()
                .RegisterCommandQueryProvider()
                .BuildServiceProvider()
                .GetRequiredService<ICommandQueryProvider>()
                .GetType()
                .Should()
                .Be(typeof(CommandQueryProvider));
        }

        [Fact(DisplayName = "RegisterCommandHandler should add proper command handler.")]
        public void RegisterCommandHandler_ShouldAddProperCommandHandler()
        {
            new ServiceCollection()
                .RegisterCommandHandler<TestCommand, TestHandlers>()
                .BuildServiceProvider()
                .GetRequiredService<ICommandHandler<TestCommand>>()
                .GetType()
                .Should()
                .Be(typeof(TestHandlers));
        }

        [Fact(DisplayName = "RegisterQueryHandler should add proper query handler.")]
        public void RegisterQueryHandler_ShouldAddProperQueryHandler()
        {
            new ServiceCollection()
                .RegisterQueryHandler<TestQuery, TestResponse, TestHandlers>()
                .BuildServiceProvider()
                .GetRequiredService<IQueryHandler<TestQuery, TestResponse>>()
                .GetType()
                .Should()
                .Be(typeof(TestHandlers));
        }

        [Fact(DisplayName = "RegisterAllCommandHandlersFromAssemblyContaining should add all command handlers.")]
        public void RegisterAllCommandHandlersFromAssemblyContaining_ShouldAddAllCommandHandlers()
        {
            new ServiceCollection()
                .RegisterAllCommandHandlersFromAssemblyContaining<TestCommand>()
                .BuildServiceProvider()
                .GetRequiredService<ICommandHandler<TestCommand>>()
                .GetType()
                .Should()
                .Be(typeof(TestHandlers));
        }

        [Fact(DisplayName = "RegisterAllQueryHandlersFromAssemblyContaining should add all query handlers.")]
        public void RegisterAllQueryHandlersFromAssemblyContaining_ShouldAddAllQueryHandlers()
        {
            new ServiceCollection()
                .RegisterAllQueryHandlersFromAssemblyContaining<TestQuery>()
                .BuildServiceProvider()
                .GetRequiredService<IQueryHandler<TestQuery, TestResponse>>()
                .GetType()
                .Should()
                .Be(typeof(TestHandlers));
        }
    }
}
