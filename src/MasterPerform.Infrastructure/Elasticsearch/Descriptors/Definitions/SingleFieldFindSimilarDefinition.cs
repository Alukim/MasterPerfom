using MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions;
using MasterPerform.Infrastructure.Entities;
using Nest;
using System;
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

        public QueryContainer GetQuery(TEntity entity)
        {
            var value = GetValue(entity);

            if (value is null)
                return null;

            var internalQuery =  new QueryContainer();

            internalQuery |= QueryBuilderExtensions.ExactMatchQuery(ExactMatchField, value);
            internalQuery |= QueryBuilderExtensions.StartsWithQuery(StartsWithField, value);
            internalQuery |= QueryBuilderExtensions.ContainsQuery(ContainsField, value);

            return internalQuery;
        }
    }
}
