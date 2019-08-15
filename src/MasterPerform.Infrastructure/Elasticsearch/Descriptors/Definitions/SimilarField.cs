using MasterPerform.Infrastructure.Entities;
using Nest;
using System.Collections.Generic;

namespace MasterPerform.Infrastructure.Elasticsearch.Descriptors.Definitions
{
    public interface ISimilarField
    {
        Field ContainsField { get; }
        Field StartsWithField { get; }
        Field ExactMatchField { get; }
    }

    public interface ISingleSimilarField<TEntity> : ISimilarField
        where TEntity : class, IEntity
    {
        string GetValueFromField(TEntity entity);
    }

    public interface ICollectionSimilarField<TEntity> : ISimilarField
        where TEntity : class, IEntity
    {
        IEnumerable<string> GetValuesFromField(TEntity entity);
    }
}
