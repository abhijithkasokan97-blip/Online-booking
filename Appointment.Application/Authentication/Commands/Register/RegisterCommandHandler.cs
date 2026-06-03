using Appointment.Application.Common.Interfaces;
using Appointment.Application.Common.Model;
using MediatR;

namespace Appointment.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{

    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService) {
        _identityService = identityService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(request.Email, request.Password, request.FirstName, request.Lastname);

        if(!result.Success)
        {
            return new AuthResponse(false, string.Join("", result.Errors));
        }
        
        return new AuthResponse(true, "Account created successfully!", Email: request.Email);

    }
}
