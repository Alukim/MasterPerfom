using MasterPerform.Infrastructure.Messaging.Contracts;
using MasterPerform.Infrastructure.Messaging.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.Messaging
{
    public interface ICommandQueryProvider
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;

        Task<TResponse> SendAsync<TQuery, TResponse>(TQuery query)
            where TQuery : IQuery<TResponse>
            where TResponse : class;
    }

    public class CommandQueryProvider : ICommandQueryProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandQueryProvider(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var commandHandler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await commandHandler.HandleAsync(command);
        }

        public async Task<TResponse> SendAsync<TQuery, TResponse>(TQuery query)
            where TQuery : IQuery<TResponse>
            where TResponse : class
        {
            var queryHandler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
            return await queryHandler.HandleAsync(query);
        }
    }
}
