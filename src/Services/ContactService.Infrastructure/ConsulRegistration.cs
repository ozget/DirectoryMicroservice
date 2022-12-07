using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.Loader;

namespace ContactService.Infrastructure
{
    public static class ConsulRegistration
    {
        public static IServiceCollection ConfigureConsul(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p=>
                new ConsulClient(consulClient =>
                {
                    var address = configuration["ConsulConfig:Address"];
                    consulClient.Address = new System.Uri(address);
                }));
            return services;
        }
        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app,IHostApplicationLifetime lifetime)
        {
            var consulClient= app.ApplicationServices.GetRequiredService<IConsulClient>();
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger=loggingFactory.CreateLogger<IApplicationBuilder>();

            //Get server Ip Address
            var features = app.Properties["server.Features"] as IFeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            //Register service with consul
            var uri = new Uri(address);
            var registration = new AgentServiceRegistration()
            {
                ID = $"ContactService",
                Name = "$ContactService",
                Address=$"{uri.Host}",
                Port = uri.Port,
                Tags = new[] { "Contact Service", "Contact" }
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();


            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}
