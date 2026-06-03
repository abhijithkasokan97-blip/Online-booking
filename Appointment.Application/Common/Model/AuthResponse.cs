
namespace Appointment.Application.Common.Model;
public record AuthResponse(
    bool Success, 
    string Message, 
    string? Email = null
);