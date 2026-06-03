using Appointment.Application.Appointments.Commands.BookAppointment;
using FluentValidation;

namespace Appointment.Application.Appointment.Commands.BookAppointment;

public class BookAppointmentValidator: AbstractValidator<BookAppointmentCommand>
{
    public BookAppointmentValidator()
    {
        RuleFor(a => a.PatientName)
            .NotEmpty().WithMessage("Patient Name cannot be empty")
            .MaximumLength(255).WithMessage("Name must not exceed 255 charachters");

        RuleFor(a => a.AppointmentTime)
            .GreaterThan(DateTime.Now)
            .WithMessage("Appointment must be future");
        
        RuleFor(v => v.DoctorName)
            .NotEmpty().WithMessage("Please select a doctor.");
    }

}
