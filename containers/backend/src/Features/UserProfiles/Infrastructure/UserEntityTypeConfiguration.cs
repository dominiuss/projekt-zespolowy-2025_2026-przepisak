using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.src.Features.UserProfile.Infrastructure
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                   .HasColumnName("id");

            builder.Property(r => r.Bio)
                   .HasColumnName("bio")
                   .HasMaxLength(50);
            builder.Property(r => r.AvatarUrl)
                   .HasColumnName("avatar_url")
                   .HasMaxLength(50);
            builder.Property(r => r.IdentityUserId)
                   .IsRequired()
                   .HasColumnName("identity_user_id");
            builder.HasOne(u => u.IdentityUser)
                   .WithMany()
                   .HasForeignKey(u => u.IdentityUserId)
                   .IsRequired();
            builder.OwnsOne(u => u.RefreshToken, rt =>
            {
                rt.Property(r => r.Token).HasColumnName("refresh_token").HasMaxLength(200);
                rt.Property(r => r.RefreshTokenExpiration).HasColumnName("refresh_token_expiration");
            });

        }
    }
}
