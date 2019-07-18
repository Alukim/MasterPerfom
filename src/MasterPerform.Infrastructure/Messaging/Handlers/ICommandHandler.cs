using MasterPerform.Infrastructure.Messaging.Contracts;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.Messaging.Handlers
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
