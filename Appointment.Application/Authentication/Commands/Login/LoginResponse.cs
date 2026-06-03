

namespace Appointment.Application.Authentication.Commands.Login;

public record LoginResponse(
    string token,
    bool Success,
    string Message
);