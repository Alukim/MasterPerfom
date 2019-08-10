using MasterPerform.Infrastructure.Entities;
using MasterPerform.Infrastructure.Exceptions;
using MasterPerform.Infrastructure.Repositories;
using Nest;
using System;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.Elasticsearch
{
    internal class ElasticsearchRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _indexName;

        public ElasticsearchRepository(IElasticClient elasticClient, IIndexNameResolver indexNameResolver)
        {
            this._elasticClient = elasticClient;
            this._indexName = indexNameResolver.GetIndexNameFor<TEntity>();
        }

        public async Task<TEntity> FindAsync(Guid id)
        {
            var response = await _elasticClient.GetAsync(DocumentPath<TEntity>.Id(id).Index(_indexName));
            return response.Source;
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            var response = await FindAsync(id);

            if (response is null)
                throw new EntityNotFound(typeof(TEntity).Name, id);

            return response;
        }

        public Task AddAsync(TEntity entity)
            => _elasticClient.IndexAsync(entity, z => z.Index(_indexName));

        public Task DeleteAsync(TEntity entity)
            => _elasticClient.DeleteAsync(DocumentPath<TEntity>.Id(entity.Id).Index(_indexName));

        public Task UpdateAsync<TPart>(TPart updatePart)
            where TPart : class, IEntity
            => _elasticClient.UpdateAsync<TEntity, TPart>(DocumentPath<TEntity>.Id(updatePart.Id),
                selector => selector
                    .Doc(updatePart)
                    .Index(_indexName));
    }
}
