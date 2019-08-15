using MasterPerform.Infrastructure.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MasterPerform.Infrastructure.Elasticsearch.Descriptors.Definitions
{
    public class SingleFieldFindSimilarDefinition<TEntity> : FindSimilarDefinition<TEntity>, ISingleSimilarField<TEntity>
        where TEntity : class, IEntity
    {
        private readonly Func<TEntity, string> GetValue;

        public SingleFieldFindSimilarDefinition(Expression<Func<TEntity, object>> field, Func<TEntity, string> getValue) : base(field)
        {

        }

        public string GetValueFromField(TEntity entity)
            => GetValue(entity);

        public override IReadOnlyCollection<QueryContainer> GetQueries()
        {
            throw new NotImplementedException();
        }
    }

    public class CollectionFieldFindSimilarDefinition<TEntity> : FindSimilarDefinition<TEntity>, ICollectionSimilarField<TEntity>
        where TEntity : class, IEntity
    {
        private readonly Func<TEntity, IEnumerable<string>> GetValues;
        private readonly Field NestedPath;

        public CollectionFieldFindSimilarDefinition(Expression<Func<TEntity, object>> field, Expression<Func<TEntity, object>> nestedPath, Func<TEntity, IEnumerable<string>> getValues) : base(field)
        {
            NestedPath = nestedPath;
            GetValues = getValues;
        }


        public IEnumerable<string> GetValuesFromField(TEntity entity)
            => GetValues(entity)?.Where(z => !string.IsNullOrEmpty(z)) ?? Array.Empty<string>();

        public override IReadOnlyCollection<QueryContainer> GetQueries()
        {
            throw new NotImplementedException();
        }
    }
}
