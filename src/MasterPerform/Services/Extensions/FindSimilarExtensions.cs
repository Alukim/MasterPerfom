using MasterPerform.Infrastructure.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions;

namespace MasterPerform.Services.Extensions
{
    public static class FindSimilarExtensions
    {
        public static QueryContainer ExcludeEntity<TEntity>(this QueryContainer container, Guid id)
            where TEntity : class, IEntity
            => container && +!new TermQuery()
            {
                Value = id,
                Field = Infer.Field<TEntity>(z => z.Id)
            };

        public static QueryContainer AddFindSimilarDefinitions<TEntity>(this QueryContainer container,
            IReadOnlyCollection<IFindSimilarDefinition<TEntity>> definitions, TEntity entity)
            where TEntity : class, IEntity
        {
            var queries = definitions?.Select(z => z.GetQuery(entity));

            if(queries is null || !queries.Any())
                return new QueryContainer();

            var boolQuery = new BoolQuery
            {
                Should = queries,
                MinimumShouldMatch = MinimumShouldMatch.Fixed(1)
            };

            return new ConstantScoreQuery
            {
                Filter = boolQuery
            };
        }
    }
}
