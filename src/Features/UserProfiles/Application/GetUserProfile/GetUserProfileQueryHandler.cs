using MapsterMapper;
using MediatR;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.UserProfiles.Application.DTOs;

namespace PrzepisakApi.src.Features.UserProfile.Application.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDTO?>
    {
        private readonly IUserProfileRepository _repository;
        private readonly IMapper _mapper;

        public GetUserProfileQueryHandler(IUserProfileRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserProfileDTO?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                return null;
            }

            return _mapper.Map<UserProfileDTO>(user);
        }
    }
}
