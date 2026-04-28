namespace ECommerce.Models
{
    public class StorefrontResponse
    {
        public List<Product> Products { get; set; } = new();
        public List<StorefrontCategoryCard> Categories { get; set; } = new();
        public StorefrontHighlightCard LeftFeature { get; set; } = new();
        public List<StorefrontHighlightCard> MosaicCards { get; set; } = new();
        public List<Product> LatestProducts { get; set; } = new();
        public List<Product> BestSellerProducts { get; set; } = new();
        public List<Product> NewCollectionProducts { get; set; } = new();
    }

    public class StorefrontCategoryCard
    {
        public string Name { get; set; } = string.Empty;
        public string Tagline { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }

    public class StorefrontHighlightCard
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }

    public class CategoryProductsResponse
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CategorySectionContent SectionConfig { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Product> FeaturedProducts { get; set; } = new();
    }

    public class CategorySectionContent
    {
        public string Slug { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FeaturedEyebrow { get; set; } = string.Empty;
        public string AllEyebrow { get; set; } = string.Empty;
        public string AllTitle { get; set; } = string.Empty;
        public string AllDescription { get; set; } = string.Empty;
        public string EmptyTitle { get; set; } = string.Empty;
        public string EmptyDescription { get; set; } = string.Empty;
    }
}