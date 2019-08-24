using MasterPerform.Infrastructure.Elasticsearch.Descriptors.Definitions;
using MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions;
using MasterPerform.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MasterPerform.Infrastructure.Elasticsearch.Descriptors
{
    public interface IFindSimilarDescriptor<TIndex>
        where TIndex : class, IEntity
    {
        IReadOnlyCollection<IFindSimilarDefinition<TIndex>> Definitions { get; }
    }

    public abstract class FindSimilarDescriptor<TIndex> : IFindSimilarDescriptor<TIndex>
        where TIndex : class, IEntity
    {
        private ICollection<IFindSimilarDefinition<TIndex>> definitions { get; set; }

        public IReadOnlyCollection<IFindSimilarDefinition<TIndex>> Definitions
            => definitions.ToList();

        protected FindSimilarDescriptor()
            => this.definitions = new List<IFindSimilarDefinition<TIndex>>();

        protected void RegisterSingleField(Expression<Func<TIndex, object>> field, Func<TIndex, string> getValue)
            => this.definitions.Add(new SingleFieldFindSimilarDefinition<TIndex>(
                field: field,
                getValue: getValue));

        protected void RegisterCollectionField(Expression<Func<TIndex, object>> field, Func<TIndex, IEnumerable<string>> getValues, Expression<Func<TIndex, object>> nestedPath = null)
            => this.definitions.Add(new CollectionFieldFindSimilarDefinition<TIndex>(
                field: field,
                getValues: getValues,
                nestedPath: nestedPath));
    }
}
