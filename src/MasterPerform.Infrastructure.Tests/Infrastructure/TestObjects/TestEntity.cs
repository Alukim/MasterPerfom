using MasterPerform.Infrastructure.Entities;
using System;

namespace MasterPerform.Tests.Infrastructure.TestObjects
{
    public class TestEntity : IEntity
    {
        public Guid Id { get; }
    }
}
