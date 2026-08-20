namespace Common_Module.Mediator.Event.Abstract;

public interface IEventPublisher
{
    Task Publish(IEvent eve);
}