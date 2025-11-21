namespace PrzepisakApi.src.Features.UserProfiles.Domain
{
    public class RefreshToken
    {
        public string? Token { get; set; }
        public DateTime? RefreshTokenExpiration { get;set; }
    }
}
