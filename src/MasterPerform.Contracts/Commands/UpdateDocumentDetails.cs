using MasterPerform.Contracts.Entities;
using MasterPerform.Infrastructure.Messaging.Contracts;
using System;

namespace MasterPerform.Contracts.Commands
{
    /// <summary>
    /// Command used to update document details.
    /// </summary>
    public class UpdateDocumentDetails : ICommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="documentId">Document id.</param>
        /// <param name="details">Document details.</param>
        public UpdateDocumentDetails(Guid documentId, DocumentDetails details)
        {
            DocumentId = documentId;
            Details = details;
        }

        /// <summary>
        /// Id of document.
        /// </summary>
        public Guid DocumentId { get; }

        /// <summary>
        /// Document details.
        /// </summary>
        public DocumentDetails Details { get; }
    }
}
