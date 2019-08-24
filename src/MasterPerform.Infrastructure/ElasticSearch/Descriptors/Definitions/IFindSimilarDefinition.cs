using Nest;
using System.Collections.Generic;

namespace MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions
{
    public interface IFindSimilarDefinition<TEntity>
    {
        IEnumerable<QueryContainer> GetQuery(TEntity entity);
    }
}
