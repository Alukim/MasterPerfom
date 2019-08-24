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
        private readonly IFindSimilarDescriptor<TEntity> _descriptor;
        private readonly IElasticClient _elasticClient;
        private readonly IIndexNameResolver _indexNameResolver;

        public FindSimilarService(
            IFindSimilarDescriptor<TEntity> descriptor,
            IElasticClient elasticClient,
            IIndexNameResolver indexNameResolver)
        {
            this._descriptor = descriptor;
            this._elasticClient = elasticClient;
            this._indexNameResolver = indexNameResolver;
        }

        public async Task<TEntity> FindSimilar(TEntity entity)
        {
            var queryContainerDescriptor = new QueryContainerDescriptor<TEntity>()
                .AddFindSimilarDefinitions(_descriptor.Definitions, entity)
                .ExcludeEntity<TEntity>(entity.Id);

            var indexName = _indexNameResolver.GetIndexNameFor<TEntity>();

            await _elasticClient.Indices.RefreshAsync(indexName);

            var results = await _elasticClient.SearchAsync<TEntity>(z => z
                .Query(x => queryContainerDescriptor)
                .Sort(x => x.Descending(SortSpecialField.Score))
                .Size(10)
                .Index(indexName));

            return results
                ?.Hits
                ?.OrderByDescending(z => z.Score)
                .FirstOrDefault()
                ?.Source;
        }
    }
}
