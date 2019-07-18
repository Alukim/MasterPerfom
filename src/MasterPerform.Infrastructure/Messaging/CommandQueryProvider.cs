using MasterPerform.Infrastructure.Messaging.Contracts;
using MasterPerform.Infrastructure.Messaging.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.Messaging
{
    public interface ICommandQueryProvider
    {
        Task SendAsync(ICommand command);

        Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query) where TResponse : class;
    }

    public class CommandQueryProvider : ICommandQueryProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandQueryProvider(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task SendAsync(ICommand command)
        {
            var commandHandler = _serviceProvider.GetRequiredService<ICommandHandler<ICommand>>();
            await commandHandler.HandleAsync(command);
        }

        public async Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query)
            where TResponse : class
        {
            var queryHandler = _serviceProvider.GetRequiredService<IQueryHandler<IQuery<TResponse>, TResponse>>();
            return await queryHandler.HandleAsync(query);
        }
    }
}
