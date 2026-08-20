using Common_Module.Mediator.Command.Abstract;
using Common_Module.Mediator.Command.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Extensions;

public static partial class ServiceCollectionExtension
{
    public static IServiceCollection AddCommandHandler<TCommand, TCommandHandler>(this IServiceCollection services)
    where TCommand : ICommand
    where TCommandHandler : class, ICommandHandler<TCommand>
    {
        services.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
        return services;
    }

    public static IServiceCollection AddCommandHandler<TCommand, TResult, TCommandHandler>(this IServiceCollection services)
    where TCommand : ICommand<TResult>
    where TCommandHandler : class, ICommandHandler<TCommand, TResult>
    {
        services.AddTransient<ICommandHandler<TCommand, TResult>, TCommandHandler>();
        return services;
    }
}