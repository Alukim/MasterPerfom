using MasterPerform.Infrastructure.Elasticsearch.Descriptors.Definitions;
using MasterPerform.Infrastructure.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions
{
    public class CollectionFieldFindSimilarDefinition<TEntity> : FindSimilarDefinition<TEntity>, IFindSimilarDefinition<TEntity>
        where TEntity : class, IEntity
    {
        private readonly Func<TEntity, IEnumerable<string>> GetValues;
        private readonly Field NestedPath;

        public CollectionFieldFindSimilarDefinition(Expression<Func<TEntity, object>> field, Func<TEntity, IEnumerable<string>> getValues, Expression<Func<TEntity, object>> nestedPath = null) : base(field)
        {
            NestedPath = nestedPath;
            GetValues = getValues;
        }


        public IEnumerable<string> GetValuesFromField(TEntity entity)
            => GetValues(entity)?.Where(z => !string.IsNullOrEmpty(z)) ?? Array.Empty<string>();

        public QueryContainer GetQuery(TEntity entity)
        {
            var values = GetValuesFromField(entity);

            if (values is null || !values.Any())
                return null;

            var queryContainer = new QueryContainer();

            foreach (var value in values)
            {
                var internalQuery = new QueryContainer();

                internalQuery |= QueryBuilderExtensions.ExactMatchQuery(ExactMatchField, value, nestedField: NestedPath);
                internalQuery |= QueryBuilderExtensions.StartsWithQuery(StartsWithField, value, nestedField: NestedPath);
                internalQuery |= QueryBuilderExtensions.ContainsQuery(ContainsField, value, nestedField: NestedPath);

                queryContainer |= internalQuery;
            }

            return queryContainer;
        }
    }
}
