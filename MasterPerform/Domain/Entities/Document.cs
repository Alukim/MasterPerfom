using MasterPerform.Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace MasterPerform.Domain.Entities
{
    public class Document : IEntity
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public IReadOnlyCollection<Address> Addresses { get; set; }
    }
}
