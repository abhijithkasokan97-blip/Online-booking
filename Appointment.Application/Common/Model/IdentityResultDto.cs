namespace Appointment.Application.Common.Model;

public record IdentityResultDto(string Id, string UserName, IEnumerable<string>Roles);
