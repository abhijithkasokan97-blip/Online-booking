using Appointment.Application.Authentication.Commands.Login;
using Appointment.Application.Authentication.Commands.Register;
using Appointment.Application.Common.Model;
using Appointment.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Appointment.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly JwtOptions _jwtOptions;

        public AuthController(
            IMediator mediator,
            IOptions<JwtOptions>  jwtOption
            )
        {
            _mediator = mediator;
            _jwtOptions = jwtOption.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>>RegisterUserAsync(RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            var  loginResponse = await _mediator.Send(command, cancellationToken);
            
            if(!loginResponse.Success){
                return Unauthorized(loginResponse.Message);
            }

            var cookieOptions =  new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryTime)
            };
            Response.Cookies.Append("X-Auth-Token", loginResponse.token, cookieOptions);

            return Ok(new { message = loginResponse.Message});
        }
    }
}
