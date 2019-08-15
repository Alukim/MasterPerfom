using System.Collections.Generic;
using MasterPerform.Infrastructure.Entities;
using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Descriptors
{
    public interface IFindSimilarDescriptor<TIndex>
        where TIndex : class, IEntity
    {
        IReadOnlyCollection<QueryContainer> GetQueries();
    }

    public abstract class FindSimilarDescriptor<TIndex> : IFindSimilarDescriptor<TIndex>
        where TIndex : class, IEntity
    {

        public IReadOnlyCollection<QueryContainer> GetQueries()
        {
            throw new System.NotImplementedException();
        }
    }
}
