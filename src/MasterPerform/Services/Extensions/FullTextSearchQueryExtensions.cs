using MasterPerform.Infrastructure.Elasticsearch.Descriptors;
using MasterPerform.Infrastructure.Entities;
using Nest;
using System.Collections.Generic;

namespace MasterPerform.Services.Extensions
{
    public static class FullTextSearchQueryExtensions
    {
        public static QueryContainer BuildFTSQuery<TIndex>(this IFullTextSearchDescriptor<TIndex> descriptor, string query)
            where TIndex : class, IEntity
        {
            var queryContainer = new QueryContainer();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var queries = new List<QueryContainer>();

                var words = query.Trim().ToLower().Split(' ');

                foreach (var word in words)
                {
                    var wordQueryContainer = new QueryContainer();

                    foreach (var descriptorDefinition in descriptor.Definitions)
                    {
                        wordQueryContainer |= descriptorDefinition(word);
                    }

                    queries.Add(wordQueryContainer);
                }

                queryContainer = new BoolQuery
                {
                    Must = queries.ToArray()
                };
            }

            return queryContainer;
        }
    }
}
