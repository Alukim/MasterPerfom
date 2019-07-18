using MasterPerform.Infrastructure.Entities;
using MasterPerform.Infrastructure.Repositories;
using Nest;
using System;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.Elasticsearch
{
    internal class ElasticsearchRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IElasticClient elasticClient;
        private readonly string indexName;

        public ElasticsearchRepository(IElasticClient elasticClient, IndexNameResolver indexNameResolver)
        {
            this.elasticClient = elasticClient;
            this.indexName = indexNameResolver.Resolve<TEntity>();
        }

        public async Task<TEntity> FindAsync(Guid id)
        {
            var response = await elasticClient.GetAsync(DocumentPath<TEntity>.Id(id).Index(indexName));
            return response.Source;
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            var response = await FindAsync(id);

            if (response is null)
                throw new Exception();

            return response;
        }

        public Task AddAsync(TEntity entity)
            => elasticClient.IndexAsync(entity, z => z.Index(indexName));

        public Task DeleteAsync(TEntity entity)
            => elasticClient.DeleteAsync(DocumentPath<TEntity>.Id(entity.Id).Index(indexName));
    }
}
