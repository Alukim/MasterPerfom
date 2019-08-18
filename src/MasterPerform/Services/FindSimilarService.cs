using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.Elasticsearch.Descriptors;
using MasterPerform.Infrastructure.Entities;
using MasterPerform.Services.Extensions;
using Nest;
using System.Linq;
using System.Threading.Tasks;

namespace MasterPerform.Services
{
    public class FindSimilarService<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IFindSimilarDescriptor<TEntity> descriptor;
        private readonly IElasticClient elasticClient;
        private readonly IIndexNameResolver indexNameResolver;

        public FindSimilarService(
            IFindSimilarDescriptor<TEntity> descriptor,
            IElasticClient elasticClient,
            IIndexNameResolver indexNameResolver)
        {
            this.descriptor = descriptor;
            this.elasticClient = elasticClient;
            this.indexNameResolver = indexNameResolver;
        }

        public async Task<TEntity> FindSimilar(TEntity entity)
        {
            var queryContainerDescriptor = new QueryContainerDescriptor<TEntity>()
                .AddFindSimilarDefinitions(descriptor.Definitions, entity)
                .ExcludeEntity<TEntity>(entity.Id);

            var results = await elasticClient.SearchAsync<TEntity>(z => z
                .Query(x => queryContainerDescriptor)
                .Sort(x => x.Descending(SortSpecialField.Score))
                .Size(10)
                .Index(indexNameResolver.GetIndexNameFor<TEntity>()));

            return results
                ?.Hits
                ?.OrderByDescending(z => z.Score)
                .FirstOrDefault()
                ?.Source;
        }
    }
}
