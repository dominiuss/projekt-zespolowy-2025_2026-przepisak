using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrzepisakApi.src.Features.Recipe.Domain;

namespace PrzepisakApi.src.Features.Recipes.Infrastructure
{
    public class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("ingredients");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                   .HasColumnName("id");

            builder.Property(i => i.Name)
                   .IsRequired()
                   .HasMaxLength(200)
                   .HasColumnName("name");

            builder.HasMany(i => i.RecipeIngredients)
                   .WithOne(ri => ri.Ingredient)
                   .HasForeignKey(ri => ri.IngredientId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_recipe_ingredients_ingredients");
        }
    }
}
