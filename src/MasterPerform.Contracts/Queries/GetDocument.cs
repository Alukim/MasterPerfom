using MasterPerform.Contracts.Responses;
using MasterPerform.Infrastructure.Messaging.Contracts;
using System;

namespace MasterPerform.Contracts.Queries
{
    /// <summary>
    /// Query used to get document.
    /// </summary>
    public class GetDocument : IQuery<DocumentResponse>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="documentId">Id of document.</param>
        public GetDocument(Guid documentId)
        {
            DocumentId = documentId;
        }

        /// <summary>
        /// Id of document.
        /// </summary>
        public Guid DocumentId { get; }
    }
}
