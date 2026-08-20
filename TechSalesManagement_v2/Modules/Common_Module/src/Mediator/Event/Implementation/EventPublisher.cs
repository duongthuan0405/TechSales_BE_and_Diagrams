using Common_Module.Mediator.Event.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Mediator.Event.Implementation;

public class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;
    public EventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task Publish(IEvent eve)
    {
        Type eventType = eve.GetType();
        Type wrapperType = typeof(IEventHandlerWrapper<>).MakeGenericType(eventType);
        IBaseEventHandlerWrapper wrapper = (IBaseEventHandlerWrapper)_serviceProvider.GetRequiredService(wrapperType);
        await wrapper.Handle(eve);
    }
}