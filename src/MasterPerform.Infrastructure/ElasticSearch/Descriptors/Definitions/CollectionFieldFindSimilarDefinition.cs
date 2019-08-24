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

        public IEnumerable<QueryContainer> GetQuery(TEntity entity)
        {
            var values = GetValuesFromField(entity)?.ToList() ?? new List<string>();

            foreach (var value in values)
            {
                if (!value.Any())
                    continue;

                yield return QueryBuilderExtensions.ExactMatchQuery(ExactMatchField, value, nestedField: NestedPath);
                yield return QueryBuilderExtensions.StartsWithQuery(StartsWithField, value, nestedField: NestedPath);
                yield return QueryBuilderExtensions.ContainsQuery(ContainsField, value, nestedField: NestedPath);
            }
        }
    }
}
