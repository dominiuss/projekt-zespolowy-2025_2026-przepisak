using MediatR;
using PrzepisakApi.api.src.Features.Auth.Application.DTOs;

namespace PrzepisakApi.api.src.Features.Auth.Application.Register;

public record RegisterCommand : IRequest<RegistrationResponseDTO>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
