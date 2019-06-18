using System;

namespace MasterPerform.Infrastructure.Messaging.Contracts
{
    public interface ICreateCommand : ICommand
    {
        Guid CreatedId { get; }
    }
}
