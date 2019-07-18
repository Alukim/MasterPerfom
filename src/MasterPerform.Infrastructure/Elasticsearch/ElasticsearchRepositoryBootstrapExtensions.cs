using MasterPerform.Infrastructure.Entities;
using MasterPerform.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.Infrastructure.Elasticsearch
{
    public static class ElasticsearchRepositoryBootstrapExtensions
    {
        public static IServiceCollection RegisterElasticsearchRepositoryFor<TEntity>(
            this IServiceCollection serviceCollection)
            where TEntity : class, IEntity
            => serviceCollection.AddScoped<IEntityRepository<TEntity>, ElasticsearchRepository<TEntity>>();
    }
}
