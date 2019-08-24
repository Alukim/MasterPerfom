using MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions;
using MasterPerform.Infrastructure.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MasterPerform.Infrastructure.Elasticsearch.Descriptors.Definitions
{
    public class SingleFieldFindSimilarDefinition<TEntity> : FindSimilarDefinition<TEntity>, IFindSimilarDefinition<TEntity>
        where TEntity : class, IEntity
    {
        private readonly Func<TEntity, string> GetValue;

        public SingleFieldFindSimilarDefinition(Expression<Func<TEntity, object>> field, Func<TEntity, string> getValue) : base(field)
        {
            GetValue = getValue;
        }

        public IEnumerable<QueryContainer> GetQuery(TEntity entity)
        {
            var value = GetValue(entity);

            yield return QueryBuilderExtensions.ExactMatchQuery(ExactMatchField, value);
            yield return QueryBuilderExtensions.StartsWithQuery(StartsWithField, value);
            yield return QueryBuilderExtensions.ContainsQuery(ContainsField, value);
        }
    }
}
