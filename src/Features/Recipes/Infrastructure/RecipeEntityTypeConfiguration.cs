using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrzepisakApi.src.Features.Recipes.Infrastructure
{
    public class RecipeEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Recipe>
    {
        public void Configure(EntityTypeBuilder<Domain.Recipe> builder)
        {
            builder.ToTable("recipes");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                   .HasColumnName("id");

            builder.Property(r => r.Title)
                   .IsRequired()
                   .HasColumnName("title")
                   .HasMaxLength(200);

            builder.Property(r => r.Description)
                   .HasColumnName("description")
                   .HasMaxLength(1000);

            builder.Property(r => r.Instructions)
                   .IsRequired()
                   .HasColumnName("instructions");

            builder.Property(r => r.PreparationTime)
                   .IsRequired()
                   .HasColumnName("preparation_time");

            builder.Property(r => r.CookTime)
                   .IsRequired()
                   .HasColumnName("cook_time");

            builder.Property(r => r.Servings)
                   .IsRequired()
                   .HasColumnName("servings");

            builder.Property(r => r.Cuisine)
                   .HasColumnName("cuisine");

            builder.Property(r => r.ImageUrl)
                   .HasColumnName("image_url");

            builder.Property(r => r.CreatedAt)
                   .HasColumnName("created_at");

            builder.Property(r => r.UpdatedAt)
                   .HasColumnName("updated_at");

            builder.Property(r => r.AuthorId)
                   .HasColumnName("author_id");
            builder.HasOne(r => r.Author)
                   .WithMany()
                   .HasForeignKey(r => r.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_recipes_users");


            builder.Property(r => r.CategoryId)
                   .HasColumnName("category_id");

            builder.HasOne(r => r.Category)
                   .WithMany(c => c.Recipes)
                   .HasForeignKey(r => r.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_recipes_categories");
        }
    }
}
