namespace PrzepisakApi.src.Features.Recipe.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> Subcategories { get; set; } = new List<Category>();
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
