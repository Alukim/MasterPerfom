﻿using MasterPerform.Contracts.Commands;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Infrastructure.Repositories;
using MasterPerform.Services;
using MasterPerform.UpdateModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MasterPerform.Handlers
{
    public class CreateDocumentCommandHandler :
        ICommandHandler<CreateDocument>
    {
        private readonly IEntityRepository<Document> _repository;
        private readonly FindSimilarService<Document> findSimilarService;

        public CreateDocumentCommandHandler(
            IEntityRepository<Document> repository,
            FindSimilarService<Document> findSimilarService)
        {
            this._repository = repository;
            this.findSimilarService = findSimilarService;
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

            if (command.FindSimilar)
            {
                var similarDocument = await findSimilarService.FindSimilar(document);
                var updateModel = new SimilarDocumentUpdate(id: document.Id, similarDocument: similarDocument?.Id);
                await _repository.UpdateAsync(updateModel);
            }
        }
    }
}
