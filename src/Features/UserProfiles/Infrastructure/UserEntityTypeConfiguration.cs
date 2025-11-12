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

            builder.Property(r => r.Username)
                   .IsRequired()
                   .HasColumnName("username")
                   .HasMaxLength(20);
        }
    }
}
