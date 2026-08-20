using Common_Module.Mediator.Command.Abstract;
using Common_Module.Mediator.Command.Implementation;
using Common_Module.Mediator.Event.Abstract;
using Common_Module.Mediator.Event.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Extensions;

public static partial class ServiceCollectionExtension
{
    public static IServiceCollection AddEventPublisher(this IServiceCollection services)
    {
        services.AddTransient<IEventPublisher, EventPublisher>();
        services.AddTransient(typeof(IEventHandlerWrapper<>), typeof(EventHandlerWrapper<>));
        
        return services;
    }
}