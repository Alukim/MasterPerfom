using MasterPerform.Contracts.Commands;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Infrastructure.Repositories;
using MasterPerform.UpdateModels;
using System.Threading.Tasks;

namespace MasterPerform.Handlers
{
    public class UpdateDocumentDetailsCommandHandler : ICommandHandler<UpdateDocumentDetails>
    {
        private readonly IEntityRepository<Document> _repository;

        public UpdateDocumentDetailsCommandHandler(IEntityRepository<Document> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(UpdateDocumentDetails command)
        {
            var document = await _repository.GetAsync(command.DocumentId);
            var updateModel = new DocumentDetailsUpdate(
                id: command.DocumentId,
                details: new DocumentDetails(
                    firstName: command.Details.FirstName,
                    lastName: command.Details.LastName,
                    email: command.Details.Email,
                    phone: command.Details.Phone));
            await _repository.UpdateAsync(updateModel);
        }
    }
}
