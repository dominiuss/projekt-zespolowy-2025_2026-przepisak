using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrzepisakApi.src.Features.Recipe.Domain;

namespace PrzepisakApi.src.Features.Recipes.Infrastructure
{
    public class RecipeIngredientEntityTypeConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.ToTable("recipe_ingredients");
            builder.HasKey(ri => ri.Id);

            builder.Property(ri => ri.Id)
                   .HasColumnName("id");

            builder.Property(ri => ri.RecipeId)
                   .HasColumnName("recipe_id")
                   .IsRequired();

            builder.Property(ri => ri.IngredientId)
                   .HasColumnName("ingredient_id")
                   .IsRequired();

            builder.Property(ri => ri.Quantity)
                   .HasColumnName("quantity")
                   .HasMaxLength(100);

            builder.HasOne(ri => ri.Recipe)
                   .WithMany(r => r.RecipeIngredients)
                   .HasForeignKey(ri => ri.RecipeId)
                   .HasConstraintName("fk_recipe_ingredients_recipes");

            builder.HasOne(ri => ri.Ingredient)
                   .WithMany(i => i.RecipeIngredients)
                   .HasForeignKey(ri => ri.IngredientId)
                   .HasConstraintName("fk_recipe_ingredients_ingredients");
        }
    }
}
