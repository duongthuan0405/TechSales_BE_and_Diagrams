namespace Common_Module.Mediator.Command.Abstract;

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}


public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult?> Handle(TCommand command, CancellationToken cancellationToken = default);
}
