using System;
using System.Collections.Generic;
using System.Text;
using MasterPerform.EntityParts;

namespace MasterPerform.UpdateModels
{
    public class SimilarDocumentUpdate : ISimilarDocumentUpdatePart
    {
        public SimilarDocumentUpdate(Guid id, Guid? similarDocument)
        {
            Id = id;
            SimilarDocument = similarDocument;
        }

        public Guid Id { get; }
        public Guid? SimilarDocument { get; }
    }
}
