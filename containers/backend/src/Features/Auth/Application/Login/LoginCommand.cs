using MediatR;
using PrzepisakApi.api.src.Features.Auth.Application.DTOs;

namespace PrzepisakApi.api.src.Features.Auth.Application.Login;

public record LoginCommand : IRequest<LoginResponse>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
