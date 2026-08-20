using Common_Module.Mediator.Command.Abstract;
using Common_Module.Mediator.Command.Implementation;
using Common_Module.Mediator.Event.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Extensions;

public static partial class ServiceCollectionExtension
{
    public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
    where TEvent : IEvent
    where TEventHandler : class, IEventHandler<TEvent>
    {
        services.AddTransient<IEventHandler<TEvent>, TEventHandler>();
        return services;
    }
}