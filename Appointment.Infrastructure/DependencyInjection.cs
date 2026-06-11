using System.Text;
using Appointment.Application.Common.Interfaces;
using Appointment.Infrastructure.Authentication;
using Appointment.Infrastructure.Identity;
using Appointment.Infrastructure.Persistant;
using Azure.Messaging.ServiceBus;
using Infrastructure.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
        
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    NameClaimType = "name",
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if(context.Request.Cookies.ContainsKey("X-Auth-Token"))
                        {
                            context.Token = context.Request.Cookies["X-Auth-Token"];
                        } 
                        
                        return Task.CompletedTask;
                    }
                };
            }
        );

            
        return services;
    }
}