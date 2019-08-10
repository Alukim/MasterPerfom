using MasterPerform.Entities;
using MasterPerform.EntityParts;
using System;
using System.Collections.Generic;

namespace MasterPerform.UpdateModels
{
    public class DocumentAddressesUpdate : IDocumentAddressUpdatePart
    {
        public DocumentAddressesUpdate(Guid id, IReadOnlyCollection<Address> addresses)
        {
            Id = id;
            Addresses = addresses;
        }

        public Guid Id { get; }
        public IReadOnlyCollection<Address> Addresses { get; }
    }
}
