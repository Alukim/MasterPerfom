using MasterPerform.Entities;
using System.Collections.Generic;

namespace MasterPerform.EntityParts
{
    public interface IAddressPart
    {
        IReadOnlyCollection<Address> Addresses { get; }
    }
}
