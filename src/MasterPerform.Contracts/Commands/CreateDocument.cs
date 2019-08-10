using MasterPerform.Contracts.Entities;
using MasterPerform.Infrastructure.Messaging.Contracts;
using System;
using System.Collections.Generic;

namespace MasterPerform.Contracts.Commands
{
    /// <summary>
    /// Command used to create new document.
    /// </summary>
    public class CreateDocument : ICreateCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="documentDetails">Document details.</param>
        /// <param name="addresses">Document addresses.</param>
        /// <param name="findSimilar">Flag indicate that operation should find similar document after creation.</param>
        public CreateDocument(
            DocumentDetails documentDetails,
            IReadOnlyCollection<Address> addresses,
            bool findSimilar)
        {
            DocumentDetails = documentDetails;
            Addresses = addresses;
            FindSimilar = findSimilar;
        }

        /// <summary>
        /// Document details.
        /// </summary>
        public DocumentDetails DocumentDetails { get; }

        /// <summary>
        /// Document addresses.
        /// </summary>
        public IReadOnlyCollection<Address> Addresses { get; }

        /// <summary>
        /// Flag indicate that operation should find similar document after creation.
        /// </summary>
        public bool FindSimilar { get; }

        /// <summary>
        /// Id of newly created id.
        /// </summary>
        public Guid CreatedId { get; set; }
    }
}
