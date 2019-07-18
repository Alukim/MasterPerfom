using MasterPerform.Infrastructure.Entities;
using System;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.Repositories
{
    public interface IEntityRepository<TEntity>
        where TEntity : class, IEntity
    {
        Task<TEntity> FindAsync(Guid id);

        Task<TEntity> GetAsync(Guid id);

        Task AddAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}
