using MediatR;

namespace Appointment.Application.Notification.Commands.SendEmail;

public record SendEmailCommand (string ToAddress, string Subject, string Body): IRequest;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
{
    public Task Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}


