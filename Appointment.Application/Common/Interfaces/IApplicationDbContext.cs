using Appointment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AppointmentRecord> Appointments {get;}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
