using MasterPerform.Contracts.Entities;
using System;
using System.Collections.Generic;

namespace MasterPerform.Contracts.Responses
{
    public class DocumentResponse
    {
        public DocumentResponse(DocumentDetails documentDetails, IReadOnlyCollection<Address> addresses, Guid? similarDocument)
        {
            DocumentDetails = documentDetails;
            Addresses = addresses;
            SimilarDocument = similarDocument;
        }

        public DocumentDetails DocumentDetails { get; }

        public IReadOnlyCollection<Address> Addresses { get; }

        public Guid? SimilarDocument { get; }
    }
}
