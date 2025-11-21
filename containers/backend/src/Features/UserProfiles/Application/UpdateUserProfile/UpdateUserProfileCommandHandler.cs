using MapsterMapper;
using MediatR;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.UserProfiles.Application.DTOs;

namespace PrzepisakApi.src.Features.UserProfile.Application.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileDTO>
    {
        private readonly IUserProfileRepository _repository;
        private readonly IMapper _mapper;

        public UpdateUserProfileCommandHandler(IUserProfileRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserProfileDTO> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                throw new Exception("User profile not found.");
            }

            user.Bio = request.Bio;
            user.AvatarUrl = request.AvatarUrl;

            if (!string.IsNullOrEmpty(request.username) && user.IdentityUser != null)
            {
                user.IdentityUser.UserName = request.username;
                user.IdentityUser.NormalizedUserName = request.username.ToUpper();
            }

            await _repository.SaveAsync(cancellationToken);

            return new UserProfileDTO
            {
                Id = user.Id,
                Username = user.IdentityUser?.UserName ?? "Błąd",
                Bio = user.Bio,
                AvatarUrl = user.AvatarUrl
            };
        }
    }
}