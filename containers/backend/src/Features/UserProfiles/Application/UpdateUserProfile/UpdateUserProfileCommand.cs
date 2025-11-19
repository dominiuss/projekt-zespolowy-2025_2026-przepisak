using MediatR;
using PrzepisakApi.src.Features.UserProfiles.Application.DTOs;

public class UpdateUserProfileCommand : IRequest<UserProfileDTO>
{
    public int UserId { get; set; }
    public string username { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
}