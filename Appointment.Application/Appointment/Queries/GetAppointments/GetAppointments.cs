using Appointment.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Application.Appointment.Queries.GetAppointments;

public record GetAppointmentsQuery : IRequest<List<AppointmentDto>>;

public class GetAppointmentsHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAppointmentsHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public Task<List<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return _context.Appointments.Select(a =>
            new AppointmentDto
            {
                Id = a.Id,
                PatientName  = a.PatientName,
                DoctorName  = a.DoctorName,
                AppointmentTime = a.AppointmentTime,
                Reason  =  a.Reason
            }
        ).ToListAsync(cancellationToken);
    }
}