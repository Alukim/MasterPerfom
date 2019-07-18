using MasterPerform.Infrastructure.Messaging.Contracts;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.Messaging.Handlers
{
    public interface IQueryHandler<TQuery, TResponse>
        where TResponse : class
        where TQuery : IQuery<TResponse>
    {
        Task<TResponse> HandleAsync(TQuery query);
    }
}
