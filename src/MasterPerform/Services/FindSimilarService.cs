using MasterPerform.Infrastructure.Elasticsearch.Descriptors;
using MasterPerform.Infrastructure.Entities;
using MasterPerform.Infrastructure.Repositories;
using MasterPerform.Services.Extensions;
using Nest;
using System;
using System.Threading.Tasks;

namespace MasterPerform.Services
{
    public class FindSimilarService<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IFindSimilarDescriptor<TEntity> descriptor;
        private readonly IEntityRepository<TEntity> repository;

        public FindSimilarService(IFindSimilarDescriptor<TEntity> descriptor, IEntityRepository<TEntity> repository)
        {
            this.descriptor = descriptor;
            this.repository = repository;
        }

        public async Task<TEntity> FindSimilar(Guid id)
        {
            var entity = await repository.GetAsync(id);

            var queryContainerDescriptor = new QueryContainerDescriptor<TEntity>()
                .ExcludeEntity<TEntity>(id);

            return entity;
        }
    }
}
