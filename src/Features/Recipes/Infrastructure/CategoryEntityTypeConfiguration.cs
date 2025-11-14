using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Infrastructure
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .HasColumnName("id");

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200)
                   .HasColumnName("name");

            builder.Property(c => c.ParentCategoryId)
                   .HasColumnName("parent_category_id");

            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.Subcategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_categories_parent_category");
        }
    }
}
