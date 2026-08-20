using Common_Module.Mediator.Command.Abstract;
using Common_Module.Mediator.Command.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Extensions;

public static partial class ServiceCollectionExtension
{
    public static IServiceCollection AddCommandExecutor(this IServiceCollection services)
    {
        services.AddTransient<ICommandExecutor, CommandExecutor>();
        services.AddTransient(typeof(ICommandHandlerWrapper<>), typeof(CommandHandlerWrapper<>));
        services.AddTransient(typeof(ICommandHandlerWrapper<,>), typeof(CommandHandlerWrapper<,>));
        return services;
    }
}