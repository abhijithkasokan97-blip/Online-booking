using Appointment.Application.Common.Interfaces;
using MediatR;

namespace Appointment.Application.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password): IRequest<LoginResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{

    private readonly IIdentityService _identityService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IIdentityService identityService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _identityService = identityService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.AuthentcateAsync(request.Email, request.Password);
        if(user ==null)
        {
            return new LoginResponse("",false, "Invalid credentials");
        }
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Roles);

        
        return new LoginResponse(token,true,"Logged In Succesfully");
    }
}