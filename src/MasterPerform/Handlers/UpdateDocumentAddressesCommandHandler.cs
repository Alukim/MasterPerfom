using MasterPerform.Contracts.Commands;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Infrastructure.Repositories;
using MasterPerform.UpdateModels;
using System.Linq;
using System.Threading.Tasks;

namespace MasterPerform.Handlers
{
    public class UpdateDocumentAddressesCommandHandler : ICommandHandler<UpdateDocumentAddresses>
    {
        private readonly IEntityRepository<Document> _repository;

        public UpdateDocumentAddressesCommandHandler(IEntityRepository<Document> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(UpdateDocumentAddresses command)
        {
            var document = await _repository.GetAsync(command.DocumentId);
            var updateModel = new DocumentAddressesUpdate(
                id: command.DocumentId,
                addresses: command.Addresses?.Select(x => new Address(
                    addressLine: x.AddressLine,
                    city: x.City,
                    state: x.State)).ToList());
            await _repository.UpdateAsync(updateModel);
        }
    }
}
