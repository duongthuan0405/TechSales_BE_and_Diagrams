using Common_Module.Mediator.Event.Abstract;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Mediator.Event.Implementation;

internal class EventHandlerWrapper<TEvent> : IEventHandlerWrapper<TEvent> where TEvent : IEvent
{
    private readonly IServiceProvider _serviceProvider;
    public EventHandlerWrapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task Handle(object eve, CancellationToken cancellationToken = default)
    {
        IEnumerable<IEventHandler<TEvent>> eventHandlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
        foreach(IEventHandler<TEvent> handler in eventHandlers)
        {
            await handler.Handle((TEvent)eve);
        }
        
    }

}