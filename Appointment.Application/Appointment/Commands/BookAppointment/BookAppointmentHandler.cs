
using Appointment.Application.Common.Interfaces;
using Appointment.Domain.Entities;
using MediatR;

namespace Appointment.Application.Appointments.Commands.BookAppointment;

public class BookAppointmentHandler : IRequestHandler<BookAppointmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMessagePublisher _messagePublisher;

    public BookAppointmentHandler(
        IApplicationDbContext context,
        IMessagePublisher messagePublisher
    )
    {
        _context = context;
        _messagePublisher = messagePublisher;
    }

    public async Task<Guid> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new AppointmentRecord
        {
            Id = Guid.NewGuid(),
            PatientName = request.PatientName,
            DoctorName = request.DoctorName,
            AppointmentTime = request.AppointmentTime,
            Reason = request.Reason,
            IsConfirmed = false
        };  

        _context.Appointments.Add(appointment);

        await _context.SaveChangesAsync(cancellationToken);

        await _messagePublisher.PublishAsync<AppointmentRecord>(appointment,"appointment-booked", cancellationToken);
        
        return appointment.Id;
    }
}