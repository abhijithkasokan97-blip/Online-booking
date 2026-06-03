namespace Appointment.Application.Appointment.Queries.GetAppointments;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentTime { get; set; }
    public string Reason { get; set; } = string.Empty;
}
