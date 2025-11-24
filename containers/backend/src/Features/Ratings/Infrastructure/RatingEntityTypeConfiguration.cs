using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrzepisakApi.src.Features.Ratings.Domain;

namespace PrzepisakApi.src.Features.Ratings.Infrastructure
{
    public class RatingEntityTypeConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("ratings");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).HasColumnName("id");
            builder.Property(r => r.RecipeId).HasColumnName("recipe_id");
            builder.Property(r => r.UserId).HasColumnName("user_id");
            builder.Property(r => r.Score).HasColumnName("score").IsRequired();
            builder.Property(r => r.Comment).HasColumnName("comment").HasMaxLength(500);
            builder.Property(r => r.CreatedAt).HasColumnName("created_at");

            builder.HasOne(r => r.Recipe)
                   .WithMany(rec => rec.Ratings)
                   .HasForeignKey(r => r.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_ratings_recipes");

            builder.HasOne(r => r.User)
                   .WithMany(u => u.Ratings)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_ratings_users");
        }
    }
}