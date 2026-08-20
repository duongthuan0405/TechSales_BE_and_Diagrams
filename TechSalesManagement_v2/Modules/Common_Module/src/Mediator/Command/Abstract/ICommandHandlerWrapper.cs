namespace Common_Module.Mediator.Command.Abstract;

internal interface IBaseCommandHandlerWrapper
{
    Task<object?> Handle(object command, CancellationToken cancellationToken = default);
}

internal interface ICommandHandlerWrapper<TCommand> : IBaseCommandHandlerWrapper where TCommand : ICommand
{
    
}

internal interface ICommandHandlerWrapper<TCommand, TResult> : IBaseCommandHandlerWrapper
    where TCommand : ICommand<TResult>
{
    
}