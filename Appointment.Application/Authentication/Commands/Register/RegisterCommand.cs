using Appointment.Application.Common.Model;
using MediatR;

namespace Appointment.Application.Authentication.Commands.Register;

public record RegisterCommand(string Email, string Password, string FirstName, string Lastname): IRequest<AuthResponse>;

