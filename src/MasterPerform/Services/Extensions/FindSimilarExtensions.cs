using MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions;
using MasterPerform.Infrastructure.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MasterPerform.Services.Extensions
{
    public static class FindSimilarExtensions
    {
        public static QueryContainer ExcludeEntity<TEntity>(this QueryContainer container, Guid id)
            where TEntity : class, IEntity
            => container && +!new TermQuery
            {
                Value = id,
                Field = Infer.Field<TEntity>(z => z.Id)
            };

        public static QueryContainer AddFindSimilarDefinitions<TEntity>(this QueryContainer container,
            IReadOnlyCollection<IFindSimilarDefinition<TEntity>> definitions, TEntity entity)
            where TEntity : class, IEntity
        {
            var queries = 
                definitions?
                    .SelectMany(z => z.GetQuery(entity))
                    .Where(z => z != null)
                    .DefaultIfEmpty()
                    .Aggregate((actual, next) => actual || next);

            return container || queries;
        }
    }
}
