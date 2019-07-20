using MasterPerform.Contracts.Queries;
using MasterPerform.Contracts.Responses;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch.Queries;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Mappers;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterPerform.Handlers
{
    public class GetDocumentsQueryHandler : IQueryHandler<GetDocuments, IReadOnlyCollection<DocumentResponse>>
    {
        private readonly IElasticsearchQueryBuilder<GetDocuments, Document> queryBuilder;
        private readonly IElasticClient elasticClient;

        public GetDocumentsQueryHandler(IElasticClient elasticClient, IElasticsearchQueryBuilder<GetDocuments, Document> queryBuilder)
        {
            this.elasticClient = elasticClient;
            this.queryBuilder = queryBuilder;
        }

        public async Task<IReadOnlyCollection<DocumentResponse>> HandleAsync(GetDocuments query)
        {
            var searchDescriptor = queryBuilder.BuildQuery(query);
            var documents = await elasticClient.SearchAsync<Document>(z => searchDescriptor);
            return documents.Documents?.Select(x => x.BuildResponse()).ToList();
        }
    }
}
