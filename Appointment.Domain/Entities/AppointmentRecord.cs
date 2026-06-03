namespace Appointment.Domain.Entities;

public class AppointmentRecord
{
    public Guid Id {get; set;}
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentTime { get; set; }
    public bool IsConfirmed { get; set; }
    public string Reason { get; set; } = string.Empty;
    
}
