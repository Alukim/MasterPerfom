using MasterPerform.Infrastructure.ElasticSearch.Mappings;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MasterPerform.Infrastructure.Elasticsearch.Descriptors.Definitions
{
    public abstract class FindSimilarDefinition<TEntity> : ISimilarField
    {
        protected FindSimilarDefinition(Expression<Func<TEntity, object>> field)
        {
            ContainsField = field;
            StartsWithField = field.AppendSuffix(CustomFields.StartWithSuffix);
            ExactMatchField = field.AppendSuffix(CustomFields.ExactMatchSuffix);
        }

        public Field ContainsField { get; }
        public Field StartsWithField { get; }
        public Field ExactMatchField { get; }

        public abstract IReadOnlyCollection<QueryContainer> GetQueries();
    }
}
