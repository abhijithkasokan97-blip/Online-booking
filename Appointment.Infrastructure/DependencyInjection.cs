using Appointment.Application.Common.Interfaces;
using Appointment.Infrastructure.Authentication;
using Appointment.Infrastructure.Identity;
using Appointment.Infrastructure.Persistant;
using Azure.Messaging.ServiceBus;
using Infrastructure.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection  AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        var isServiceBusEnabled = configuration.GetValue<bool>("ServiceBusSettings:IsEnabled");
        if(isServiceBusEnabled)
        {
            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBus");
            services.AddSingleton(sp => new ServiceBusClient(serviceBusConnectionString));
            services.AddScoped<IMessagePublisher, AzureMessagePublisher>();
            services.AddHostedService<AzureServiceBusConsumer>();
        } else
        {
            services.AddSingleton<IMessagePublisher, NullMessagePublisher>();
        }
        
        
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IIdentityService, IdentityService>();

            
        return services;
    }
}