
using System.ComponentModel.DataAnnotations;

namespace Appointment.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    [Required]
    public required string Key {get; set;}
    [Required]
    public required string Issuer { get; set; }
    [Required]
    public required string Audience { get; set; }
    [Required]
    public int ExpiryTime { get; set; }

}