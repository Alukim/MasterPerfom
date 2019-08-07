using MasterPerform.Contracts.Entities;
using MasterPerform.Infrastructure.Messaging.Contracts;
using System;
using System.Collections.Generic;

namespace MasterPerform.Contracts.Commands
{
    /// <summary>
    /// Command used to update document addresses.
    /// </summary>
    public class UpdateDocumentAddresses : ICommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="documentId">Document id.</param>
        /// <param name="addresses">Document addresses.</param>
        public UpdateDocumentAddresses(Guid documentId, IReadOnlyCollection<Address> addresses)
        {
            DocumentId = documentId;
            Addresses = addresses;
        }

        /// <summary>
        /// Id of document.
        /// </summary>
        public Guid DocumentId { get; }

        /// <summary>
        /// Document addresses.
        /// </summary>
        public IReadOnlyCollection<Address> Addresses { get; }
    }
}
