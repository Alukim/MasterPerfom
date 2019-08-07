using MasterPerform.Infrastructure.ElasticSearch.Mappings;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MasterPerform.Infrastructure.Elasticsearch.Descriptors
{
    public interface IFullTextSearchDescriptor<TIndex>
        where TIndex : class
    {
        List<Func<string, QueryContainer>> Definitions { get; }
    }

    public abstract class FullTextSearchDescriptor<TIndex> : IFullTextSearchDescriptor<TIndex>
        where TIndex : class
    {
        protected FullTextSearchDescriptor()
        {
            Definitions = new List<Func<string, QueryContainer>>();
        }

        protected void RegisterFullTextSearchDefinition(Expression<Func<TIndex, object>> path)
            => Definitions.Add(ContainsQuery(path));

        protected void RegisterNestedFullTextSearchDefinition(Expression<Func<TIndex, object>> path, Expression<Func<TIndex, object>> nestedPath)
        {
            Definitions.Add(z => new NestedQuery
            {
                Path = Infer.Field(nestedPath),
                Query = ContainsQuery(path)(z)
            });
        }

        private Func<string, QueryContainer> ContainsQuery(Expression<Func<TIndex, object>> path)
        {
            return z =>
            {
                var queryContainer = new QueryContainer();

                queryContainer &= new MatchQuery
                {
                    Field = path,
                    Query = z
                };

                queryContainer |= new TermQuery
                {
                    Field = path.AppendSuffix(CustomFields.ExactMatchSuffix),
                    Value = z
                };

                return queryContainer;
            };
        }

        public List<Func<string, QueryContainer>> Definitions { get; }
    }
}
