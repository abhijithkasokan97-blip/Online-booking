namespace Appointment.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string userName, IEnumerable<string> roles);
}
