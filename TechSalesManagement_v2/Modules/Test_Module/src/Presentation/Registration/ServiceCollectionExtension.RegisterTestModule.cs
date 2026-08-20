using Common_Module.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test_Module.Test.Command;

namespace Test_Module.Extensions
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterTestModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddCommandHandler<TestCommand1, TestCommand1Handler>();
            services.AddCommandHandler<TestCommand2, string, TestCommand2Handler>();

            services.AddEventHandler<TestEvent, TestEvent1Handler>();
            services.AddEventHandler<TestEvent, TestEvent2Handler>();           
            services.AddEventHandler<TestEvent, TestEvent3Handler>();
            return services;
        }
    }
}