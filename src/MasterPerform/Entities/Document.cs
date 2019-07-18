using MasterPerform.EntityParts;
using MasterPerform.Infrastructure.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MasterPerform.Entities
{
    public class Document :
        IEntity,
        ICreateDocumentPart
    {
        public Document(Guid id, DocumentDetails details, IReadOnlyCollection<Address> addresses, Guid? similarDocument)
        {
            Id = id;
            Details = details;
            Addresses = addresses;
            SimilarDocument = similarDocument;
        }

        public Guid Id { get; }

        public DocumentDetails Details { get; }

        public IReadOnlyCollection<Address> Addresses { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public Guid? SimilarDocument { get; }
    }
}
