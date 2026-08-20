using System.Reflection;
using Common_Module.Mediator.Command.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Mediator.Command.Implementation;

public class CommandExecutor : ICommandExecutor
{
    private readonly IServiceProvider _serviceProvider;
    public CommandExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult?> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        Type commandType = command.GetType();
        Type handlerWrapperType = typeof(ICommandHandlerWrapper<,>).MakeGenericType(commandType, typeof(TResult));
        IBaseCommandHandlerWrapper baseHandlerWrapper = (IBaseCommandHandlerWrapper)(_serviceProvider.GetRequiredService(handlerWrapperType));
        object? res = await baseHandlerWrapper.Handle(command, cancellationToken);
        return res == null ? default(TResult) : (TResult)res;
    }

    public async Task Execute(ICommand command, CancellationToken cancellationToken = default)
    {
        Type commandType = command.GetType();
        Type handlerWrapperType = typeof(ICommandHandlerWrapper<>).MakeGenericType(commandType);
        IBaseCommandHandlerWrapper baseHandlerWrapper = (IBaseCommandHandlerWrapper)(_serviceProvider.GetRequiredService(handlerWrapperType));
        await baseHandlerWrapper.Handle(command, cancellationToken);
    }
}