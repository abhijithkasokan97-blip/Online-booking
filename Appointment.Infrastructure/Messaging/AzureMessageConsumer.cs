
using System.Text.Json;
using Appointment.Application.Notification.Commands.SendEmail;
using Appointment.Domain.Entities;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging;

public class AzureServiceBusConsumer : BackgroundService
{

    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<AzureServiceBusConsumer> _logger;
    private readonly ServiceBusClient  _client;
    private readonly IServiceProvider  _serviceProvider;
    
    public AzureServiceBusConsumer(
        ILogger<AzureServiceBusConsumer> logger,
        ServiceBusClient client,
        IServiceProvider  serviceProvider
    )
    {
        _logger = logger;
        _client = client;
        _serviceProvider = serviceProvider;

        var topicName = "appointment-booked";
        var subscriptionName = "send-email-worker";
        _processor = _client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 2
        });
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += HandleMessageAsync;
        _processor.ProcessErrorAsync += HandleErrorAsync;

        _logger.LogInformation("Starting Service bus consumer");


        await _processor.StartProcessingAsync(stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleMessageAsync(ProcessMessageEventArgs args)
    {


        Console.WriteLine($"!!! DEBUG: RAW MESSAGE RECEIVED: {args.Message.Body}");
        _logger.LogWarning("!!! DEBUG: RAW MESSAGE RECEIVED: {Body}", args.Message.Body.ToString());
        string rawBody = args.Message.Body.ToString();

        // if(!args.Message.ApplicationProperties.TryGetValue("MessageType", out var messageTypeObj) ||
        //     messageTypeObj is not string messageType )
        // {
        //     _logger.LogWarning("Received message missing 'MessageType' header property. Moving to Dead-Letter.");
        //     await args.DeadLetterMessageAsync(args.Message, "Missing  MessageType header");
        //     return;
        // }

        // _logger.LogInformation("Recive message of type : {messageType}", messageType);

        try
        {

            var content = JsonSerializer.Deserialize<object>(rawBody);
            _logger.LogInformation("Deserilized content {content}", content);

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                SendEmailCommand command = new SendEmailCommand("toaddress", "Subject", "body");
                await mediator.Send(command, args.CancellationToken);
            }

            await args.CompleteMessageAsync(args.Message);
        }   
        catch(Exception ex)
        {
            
            // _logger.LogError(ex, "Error executing message handler for {MessageType}.", messageType);
    
            // Release the message back to the queue so it can be retried safely
            await args.AbandonMessageAsync(args.Message);
        }

        
    }


    public Task HandleErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Service bus error. source: {source}", args.ErrorSource);
        
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync();
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }

}