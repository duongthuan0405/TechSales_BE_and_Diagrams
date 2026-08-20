namespace Common_Module.Mediator.Event.Abstract;

public interface IBaseEventHandlerWrapper
{
    Task Handle(object eve, CancellationToken cancellationToken = default);
}

public interface IEventHandlerWrapper<TEvent> : IBaseEventHandlerWrapper where TEvent : IEvent
{
    
}