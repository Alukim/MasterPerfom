using MasterPerform.Infrastructure.Messaging.Contracts;
using MasterPerform.Infrastructure.Messaging.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.Infrastructure.Messaging
{
    public static class BootstrapExtensions
    {
        public static IServiceCollection RegisterCommandQueryProvider(this IServiceCollection serviceCollection)
            => serviceCollection.AddScoped<ICommandQueryProvider, CommandQueryProvider>();

        public static IServiceCollection RegisterCommandHandler<TCommand, THandler>(
            this IServiceCollection serviceCollection)
            where TCommand : class, ICommand
            where THandler : class, ICommandHandler<TCommand>
            => serviceCollection.AddTransient<ICommandHandler<TCommand>, THandler>();

        public static IServiceCollection RegisterQueryHandler<TQuery, TResponse, THandler>(
            this IServiceCollection serviceCollection)
            where TResponse : class
            where TQuery : class, IQuery<TResponse>
            where THandler : class, IQueryHandler<TQuery, TResponse>
            => serviceCollection.AddTransient<IQueryHandler<TQuery, TResponse>, THandler>();

        public static IServiceCollection RegisterAllCommandHandlersFromAssemblyContaining<TService>(
            this IServiceCollection serviceCollection)
            => serviceCollection.Scan(z => z
                .FromAssemblyOf<TService>()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        public static IServiceCollection RegisterAllQueryHandlersFromAssemblyContaining<TService>(
            this IServiceCollection serviceCollection)
            => serviceCollection.Scan(z => z
                .FromAssemblyOf<TService>()
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
    }
}
