using System.Threading.Tasks;
using MasterPerform.Infrastructure.Messaging.Contracts;

namespace MasterPerform.Infrastructure.Messaging.Handlers
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task Handler(TCommand command);
    }
}
