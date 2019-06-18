using MasterPerform.Infrastructure.Messaging.Contracts;
using MasterPerform.WebApi.Contracts.Entities;
using System;

namespace MasterPerform.WebApi.Contracts
{
    public class CreateDocument : ICreateCommand
    {
        public CreateDocument(Document document, Guid createdId)
        {
            Document = document;
            CreatedId = createdId;
        }

        public Document Document { get; }
        public Guid CreatedId { get; }
    }
}
