using Appointment.Application.Common.Interfaces;
using Appointment.Application.Common.Model;
using Microsoft.AspNetCore.Identity;

namespace Appointment.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResultDto> AuthentcateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if(user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new IdentityResultDto(user.Id, user.UserName!, roles);
    }

    public async Task<InternalIdentityResult> CreateUserAsync(string email, string password, string firstName, string lastName)
    {
        var user = new ApplicationUser()
        {
            Email = email,
            UserName = email,
            FirstName= firstName,
            LastName = lastName  
        };

        IdentityResult result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return new InternalIdentityResult(true,user.Id, Array.Empty<string>());
        }

        return new InternalIdentityResult (false, null, result.Errors.Select(e => e.Description));
    }
}
