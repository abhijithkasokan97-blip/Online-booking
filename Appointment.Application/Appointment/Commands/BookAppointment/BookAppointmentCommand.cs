using MediatR;

namespace Appointment.Application.Appointments.Commands.BookAppointment;

public record BookAppointmentCommand(
    string PatientName,
    string DoctorName,
    DateTime AppointmentTime,
    string Reason
    ) : IRequest<Guid>;
