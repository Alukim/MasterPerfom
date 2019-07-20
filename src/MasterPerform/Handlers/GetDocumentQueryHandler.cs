using MasterPerform.Contracts.Queries;
using MasterPerform.Contracts.Responses;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Infrastructure.Repositories;
using MasterPerform.Mappers;
using System.Threading.Tasks;

namespace MasterPerform.Handlers
{
    public class GetDocumentQueryHandler : IQueryHandler<GetDocument, DocumentResponse>
    {

        private readonly IEntityRepository<Document> repository;

        public GetDocumentQueryHandler(IEntityRepository<Document> repository)
        {
            this.repository = repository;
        }

        public async Task<DocumentResponse> HandleAsync(GetDocument query)
        {
            var document = await repository.GetAsync(query.DocumentId);
            return document.BuildResponse();
        }
    }
}
