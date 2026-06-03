using Appointment.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

public class NullMessagePublisher : IMessagePublisher
{
    private readonly ILogger<NullMessagePublisher> _logger;
    public NullMessagePublisher(ILogger<NullMessagePublisher> logger)
    {
        _logger = logger;
    }
    public Task PublishAsync<T>(T message, string topicName, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Service Bus is DISABLED. Message :-{message} was skipped.", message);
        return Task.CompletedTask;
    }
}