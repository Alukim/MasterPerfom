using MasterPerform.Infrastructure.Entities;
using Nest;
using System;

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
    }
}
