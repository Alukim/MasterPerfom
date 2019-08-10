using MasterPerform.Contracts.Queries;
using MasterPerform.Contracts.Responses;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch;
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
        private readonly IElasticsearchQueryBuilder<GetDocuments, Document> _queryBuilder;
        private readonly IElasticClient _elasticClient;
        private readonly IIndexNameResolver indexNameResolver;

        public GetDocumentsQueryHandler(IElasticClient elasticClient, IElasticsearchQueryBuilder<GetDocuments, Document> queryBuilder, IIndexNameResolver indexNameResolver)
        {
            this._elasticClient = elasticClient;
            this._queryBuilder = queryBuilder;
            this.indexNameResolver = indexNameResolver;
        }

        public async Task<IReadOnlyCollection<DocumentResponse>> HandleAsync(GetDocuments query)
        {
            var searchDescriptor = _queryBuilder.BuildQuery(query);
            var documents = await _elasticClient.SearchAsync<Document>(z => searchDescriptor.Index(indexNameResolver.GetIndexNameFor<Document>()));
            return documents.Documents?.Select(x => x.BuildResponse()).ToList();
        }
    }
}
