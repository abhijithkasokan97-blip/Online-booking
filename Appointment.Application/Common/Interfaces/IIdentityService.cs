using Appointment.Application.Common.Model;

namespace Appointment.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<InternalIdentityResult>CreateUserAsync( 
        string email, string password, string firstName, string lastName);
    Task<IdentityResultDto>AuthentcateAsync(string email, string password);
}
