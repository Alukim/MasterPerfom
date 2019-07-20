using MasterPerform.Contracts.Queries;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch.Queries;
using Nest;
using System;
using System.Collections.Generic;

namespace MasterPerform.QueryBuilders
{
    internal class GetDocumentsQueryBuilder : IElasticsearchQueryBuilder<GetDocuments, Document>
    {
        public SearchDescriptor<Document> BuildQuery(GetDocuments container)
        {
            var queryContainer = new QueryContainer();

            if (!string.IsNullOrWhiteSpace(container.Query))
            {
                var queries = new List<QueryContainer>();


            }

            return new SearchDescriptor<Document>().Query(z => queryContainer);
        }
    }
}
