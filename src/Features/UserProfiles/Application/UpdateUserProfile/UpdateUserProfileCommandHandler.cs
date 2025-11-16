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
            var user = await _repository.GetByUserIdAsync(request.UserId, cancellationToken)
                       ?? await _repository.CreateForUserAsync(request.UserId, cancellationToken);

            await _repository.SaveAsync(cancellationToken);

            return _mapper.Map<UserProfileDTO>(user);
        }
    }
}
