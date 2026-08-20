namespace Common_Module.Mediator.Event.Abstract;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    Task Handle(TEvent eve, CancellationToken cancellationToken = default);
}