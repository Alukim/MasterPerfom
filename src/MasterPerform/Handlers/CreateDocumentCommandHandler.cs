using MasterPerform.Contracts.Commands;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MasterPerform.Handlers
{
    public class CreateDocumentCommandHandler :
        ICommandHandler<CreateDocument>
    {
        private readonly IEntityRepository<Document> _repository;

        public CreateDocumentCommandHandler(IEntityRepository<Document> repository)
        {
            this._repository = repository;
        }

        public async Task HandleAsync(CreateDocument command)
        {
            var document = new Document(
                id: Guid.NewGuid(),
                details: new DocumentDetails(
                    firstName: command.DocumentDetails.FirstName,
                    lastName: command.DocumentDetails.LastName,
                    email: command.DocumentDetails.Email,
                    phone: command.DocumentDetails.Phone),
                addresses: command.Addresses?.Select(x => new Address(
                    addressLine: x.AddressLine,
                    city: x.City,
                    state: x.State)).ToList(),
                similarDocument: null);

            await _repository.AddAsync(document);
            command.CreatedId = document.Id;
        }
    }
}
