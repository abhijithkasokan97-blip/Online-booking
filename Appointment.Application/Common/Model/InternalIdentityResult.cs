namespace Appointment.Application.Common.Model;

public record InternalIdentityResult(
    bool Success, 
    string? UserId, 
    IEnumerable<string> Errors
);