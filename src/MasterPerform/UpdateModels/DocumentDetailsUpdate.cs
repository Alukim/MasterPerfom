using MasterPerform.Entities;
using MasterPerform.EntityParts;
using System;

namespace MasterPerform.UpdateModels
{
    public class DocumentDetailsUpdate : IDocumentDetailsUpdatePart
    {
        public DocumentDetailsUpdate(Guid id, DocumentDetails details)
        {
            Id = id;
            Details = details;
        }

        public Guid Id { get; }
        public DocumentDetails Details { get; }
    }
}
