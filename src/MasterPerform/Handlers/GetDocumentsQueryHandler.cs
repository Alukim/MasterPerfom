using MasterPerform.Contracts.Queries;
using MasterPerform.Contracts.Responses;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.Elasticsearch.Descriptors;
using MasterPerform.Infrastructure.Elasticsearch.Queries;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Mappers;
using MasterPerform.Services.Extensions;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterPerform.Handlers
{
    public class GetDocumentsQueryHandler : IQueryHandler<GetDocuments, IReadOnlyCollection<DocumentResponse>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IIndexNameResolver indexNameResolver;
        private readonly IFullTextSearchDescriptor<Document> descriptor;

        public GetDocumentsQueryHandler(
            IElasticClient elasticClient,
            IIndexNameResolver indexNameResolver,
            IFullTextSearchDescriptor<Document> descriptor)
        {
            this._elasticClient = elasticClient;
            this.indexNameResolver = indexNameResolver;
            this.descriptor = descriptor;
        }

        public async Task<IReadOnlyCollection<DocumentResponse>> HandleAsync(GetDocuments query)
        {
            var queryContainer = descriptor.BuildFTSQuery(query.Query);
            var searchDescriptor = new SearchDescriptor<Document>()
                .AddPaging(query.PageSize, query.PageNumber)
                .Query(z => queryContainer);

            var documents = await _elasticClient.SearchAsync<Document>(z => searchDescriptor.Index(indexNameResolver.GetIndexNameFor<Document>()));

            return documents.Documents?.Select(x => x.BuildResponse()).ToList();
        }
    }
}
