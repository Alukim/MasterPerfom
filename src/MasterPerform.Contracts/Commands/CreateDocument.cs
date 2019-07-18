using MasterPerform.Contracts.Entities;
using MasterPerform.Infrastructure.Messaging.Contracts;
using System;
using System.Collections.Generic;

namespace MasterPerform.Contracts.Commands
{
    public class CreateDocument : ICreateCommand
    {
        public CreateDocument(
            DocumentDetails documentDetails,
            IReadOnlyCollection<Address> addresses)
        {
            DocumentDetails = documentDetails;
            Addresses = addresses;
        }

        public DocumentDetails DocumentDetails { get; }

        public IReadOnlyCollection<Address> Addresses { get; }

        public Guid CreatedId { get; set; }
    }
}
