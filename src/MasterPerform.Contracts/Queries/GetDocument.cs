using MasterPerform.Contracts.Responses;
using MasterPerform.Infrastructure.Messaging.Contracts;
using System;

namespace MasterPerform.Contracts.Queries
{
    public class GetDocument : IQuery<DocumentResponse>
    {
        public GetDocument(Guid documentId)
        {
            DocumentId = documentId;
        }

        public Guid DocumentId { get; }
    }
}
