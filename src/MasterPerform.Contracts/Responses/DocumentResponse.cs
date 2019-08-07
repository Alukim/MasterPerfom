using MasterPerform.Contracts.Entities;
using System;
using System.Collections.Generic;

namespace MasterPerform.Contracts.Responses
{
    /// <summary>
    /// Representation of document response.
    /// </summary>
    public class DocumentResponse
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="documentId">Id of document.</param>
        /// <param name="documentDetails">Document details.</param>
        /// <param name="addresses">Document addresses.</param>
        /// <param name="similarDocument">Id of similar document.</param>
        public DocumentResponse(Guid documentId, DocumentDetails documentDetails, IReadOnlyCollection<Address> addresses, Guid? similarDocument)
        {
            DocumentId = documentId;
            DocumentDetails = documentDetails;
            Addresses = addresses;
            SimilarDocument = similarDocument;
        }

        /// <summary>
        /// Id of document.
        /// </summary>
        public Guid DocumentId { get; }

        /// <summary>
        /// Document details.
        /// </summary>
        public DocumentDetails DocumentDetails { get; }

        /// <summary>
        /// Document addresses.
        /// </summary>
        public IReadOnlyCollection<Address> Addresses { get; }

        /// <summary>
        /// Id of similar document.
        /// </summary>
        public Guid? SimilarDocument { get; }
    }
}
