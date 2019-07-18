using MasterPerform.Entities;
using System.Collections.Generic;

namespace MasterPerform.EntityParts
{
    internal interface IAddressPart
    {
        IReadOnlyCollection<Address> Addresses { get; }
    }
}
