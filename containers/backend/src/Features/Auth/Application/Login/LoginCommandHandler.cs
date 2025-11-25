using MediatR;
using PrzepisakApi.api.src.Features.Auth.Application.DTOs;
using PrzepisakApi.api.src.Features.Auth.Services;
using PrzepisakApi.src.Features.Auth.Domain;

namespace PrzepisakApi.api.src.Features.Auth.Application.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        if (user == null)
            return new LoginResponse
            {
                ErrorMessage = "Nieprawidłowa nazwa użytkownika lub/i hasło"
            };

        var passwordValid = await _userRepository.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return new LoginResponse
            {
                ErrorMessage = "Nieprawidłowa nazwa użytkownika lub/i hasło"
            };

        var profile = await _userRepository.GetUserProfileAsync(user.Id);
        if (profile == null)
            return null;

        var tokenResponse = await _tokenService.GenerateTokensAsync(user, profile.Id);

        return new LoginResponse
        {
            Token = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            Expiration = DateTime.UtcNow.AddHours(2),
            ErrorMessage = null
        };
    }
}
