using MasterPerform.Infrastructure.Messaging.Contracts;
using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.Infrastructure.Messaging.ServiceBus;
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
        private readonly ServiceBusSender _serviceBusSender;

        public CommandQueryProvider(IServiceProvider serviceProvider, ServiceBusSender serviceBusSender)
        {
            this._serviceProvider = serviceProvider;
            this._serviceBusSender = serviceBusSender;
        }

        public async Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var commandHandler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await commandHandler.HandleAsync(command);
            await _serviceBusSender.SendMessage(command);
        }

        public async Task<TResponse> SendAsync<TQuery, TResponse>(TQuery query)
            where TQuery : IQuery<TResponse>
            where TResponse : class
        {
            var queryHandler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
            var response = await queryHandler.HandleAsync(query);
            await _serviceBusSender.SendMessage(query);
            return response;
        }
    }
}
