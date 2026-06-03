namespace Appointment.Application.Common.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string topicName, CancellationToken cancellationToken);
}