using MasterPerform.Contracts.Queries;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch.Descriptors;
using MasterPerform.Infrastructure.Elasticsearch.Queries;
using Nest;
using System.Collections.Generic;

namespace MasterPerform.QueryBuilders
{
    internal class GetDocumentsQueryBuilder : IElasticsearchQueryBuilder<GetDocuments, Document>
    {
        private readonly IFullTextSearchDescriptor<Document> _descriptor;

        public GetDocumentsQueryBuilder(IFullTextSearchDescriptor<Document> descriptor)
        {
            this._descriptor = descriptor;
        }

        public SearchDescriptor<Document> BuildQuery(GetDocuments query)
        {
            var queryContainer = new QueryContainer();

            if (!string.IsNullOrWhiteSpace(query.Query))
            {
                var queries = new List<QueryContainer>();

                var words = query.Query.Trim().ToLower().Split(' ');

                foreach (var word in words)
                {
                    var wordQueryContainer = new QueryContainer();

                    foreach (var descriptorDefinition in _descriptor.Definitions)
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

            return new SearchDescriptor<Document>()
                .AddPaging(query.PageSize, query.PageNumber)
                .Query(z => queryContainer);
        }
    }
}
