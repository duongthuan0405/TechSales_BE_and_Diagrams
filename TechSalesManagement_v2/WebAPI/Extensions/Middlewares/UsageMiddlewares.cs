using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions.ServiceProviderExtensions.Middlewares
{
    public static partial class WebApplicationExtension
    {
        public static WebApplication UseMiddlewares(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseAuthorization();

            return app;
        }
    }
}