using Common_Module.Mediator.Command.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Mediator.Command.Implementation;

internal class CommandHandlerWrapper<TCommand> : ICommandHandlerWrapper<TCommand> where TCommand : ICommand
{
    private IServiceProvider _serviceProvider;
    public CommandHandlerWrapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<object?> Handle(object command, CancellationToken cancellationToken = default)
    {
        ICommandHandler<TCommand> handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.Handle((TCommand)command, cancellationToken);
        return null;
    }
}


internal class CommandHandlerWrapper<TCommand, TResult> : ICommandHandlerWrapper<TCommand, TResult> where TCommand : ICommand<TResult>
{
    private IServiceProvider _serviceProvider;
    public CommandHandlerWrapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<object?> Handle(object command, CancellationToken cancellationToken = default)
    {
        ICommandHandler<TCommand, TResult> handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.Handle((TCommand)command, cancellationToken);
    }
}