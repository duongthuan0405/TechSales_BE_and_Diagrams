namespace Common_Module.Mediator.Command.Abstract;

public interface ICommandExecutor
{
    Task<TResult?> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task Execute(ICommand command, CancellationToken cancellationToken = default);
}