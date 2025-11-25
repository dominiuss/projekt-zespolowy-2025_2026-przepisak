using MediatR;
using PrzepisakApi.api.src.Features.Auth.Application.DTOs;
using PrzepisakApi.src.Features.Auth.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.api.src.Features.Auth.Application.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegistrationResponseDTO>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegistrationResponseDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var identityUser = await _userRepository.CreateUserAsync(request.Username, request.Password);

        var profile = new User
        {
            IdentityUserId = identityUser.Id,
        };

        _userRepository.AddUserProfile(profile);
        await _userRepository.SaveChangesAsync();

        return new RegistrationResponseDTO
        {
            Message = "Rejestracja zakończona pomyślnie."
        };
    }
}
