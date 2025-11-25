using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.Auth.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class RegistrationRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
