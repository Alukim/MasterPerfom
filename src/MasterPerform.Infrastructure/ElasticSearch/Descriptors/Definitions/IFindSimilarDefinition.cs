using Nest;

namespace MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions
{
    public interface IFindSimilarDefinition<TEntity>
    {
        QueryContainer GetQuery(TEntity entity);
    }
}
