using MasterPerform.Contracts.Commands;
using MasterPerform.Contracts.Entities;
using System;
using System.Collections.Generic;

namespace MasterPerform.Tests.Factories
{
    public class DocumentFactory
    {
        public CreateDocument GenerateSimpleCreateDocument(bool findSimilar = false)
            => GenerateCreateDocument(
                details: new DocumentDetails(
                    firstName: "John",
                    lastName: "Smith",
                    email: "john.smith@gmail.com",
                    phone: "12345467898"),
                addresses: new List<Address>
                {
                    new Address(addressLine: "Chorzowska 148", city: "Katowice", state: "Śląsk"),
                    new Address(addressLine: "Przewozowa 32", city: "Gliwice", state: "Śląsk")
                },
                findSimilar: findSimilar);

        public CreateDocument GenerateCreateDocument(DocumentDetails details, IReadOnlyCollection<Address> addresses = null, bool findSimilar = false)
        {
            return new CreateDocument(
                documentDetails: details,
                addresses: addresses,
                findSimilar: findSimilar);
        }

        public UpdateDocumentDetails GenerateUpdateDocumentDetails(Guid documentId, DocumentDetails details)
            => new UpdateDocumentDetails(
                documentId: documentId,
                details: details);

        public UpdateDocumentAddresses GenerateUpdateDocumentAddresses(Guid documentId, IReadOnlyCollection<Address> addresses)
            => new UpdateDocumentAddresses(
                documentId: documentId,
                addresses: addresses);
    }
}
