using MasterPerform.Contracts.Entities;
using System;
using System.Collections.Generic;

namespace MasterPerform.Contracts.Responses
{
    public class DocumentResponse
    {
        public DocumentResponse(Guid documentId, DocumentDetails documentDetails, IReadOnlyCollection<Address> addresses, Guid? similarDocument)
        {
            DocumentId = documentId;
            DocumentDetails = documentDetails;
            Addresses = addresses;
            SimilarDocument = similarDocument;
        }

        public Guid DocumentId { get; }

        public DocumentDetails DocumentDetails { get; }

        public IReadOnlyCollection<Address> Addresses { get; }

        public Guid? SimilarDocument { get; }
    }
}
