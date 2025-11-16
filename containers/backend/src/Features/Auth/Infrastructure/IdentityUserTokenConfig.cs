using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrzepisakApi.api.src.Features.Auth.Infrastructure;

public class IdentityUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

    }
}
