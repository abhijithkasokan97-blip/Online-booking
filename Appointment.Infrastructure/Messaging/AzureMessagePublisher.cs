using System.Text.Json;
using Appointment.Application.Common.Interfaces;
using Azure.Messaging.ServiceBus;

namespace Infrastructure.Messaging;

public class AzureMessagePublisher : IMessagePublisher
{
    private readonly ServiceBusClient _serviceBusClient;
    public AzureMessagePublisher(ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }
    public async Task PublishAsync<T>(T message, string topicName, CancellationToken cancellationToken)
    {
       ServiceBusSender sender = _serviceBusClient.CreateSender(topicName);

       string jsonBody = JsonSerializer.Serialize(message);

       ServiceBusMessage serviceBusMessage = new ServiceBusMessage(jsonBody)
       {
           ContentType = "application/json"
       };

       try 
       {
        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
       }
        catch
        {
            Console.Write("error ");    
        }
        finally
        {
            await sender.DisposeAsync();
        }       

    }
}