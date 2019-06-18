using MasterPerform.Infrastructure.Messaging.Contracts;
using MasterPerform.Infrastructure.Messaging.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.Infrastructure.Messaging
{
    public static class BootstrapExtensions
    {
        public static IServiceCollection RegisterCommandHandler<TCommand, THandler>(
            this IServiceCollection serviceCollection)
            where TCommand : class, ICommand
            where THandler : class, ICommandHandler<TCommand>
            => serviceCollection.AddTransient<ICommandHandler<TCommand>, THandler>();
    }
}
