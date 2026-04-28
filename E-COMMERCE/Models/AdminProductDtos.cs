using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class ProductImageUpsertRequest
    {
        [Required]
        [Url]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsMain { get; set; }
    }

    public class ProductUpsertRequest
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Sku { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [StringLength(200)]
        public string Category { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public bool IsNew { get; set; }

        public List<ProductImageUpsertRequest> ProductImages { get; set; } = new();
    }

    public class StorefrontSectionUpsertRequest
    {
        [Required]
        [StringLength(200)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FeaturedEyebrow { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string AllEyebrow { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string AllTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string AllDescription { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string EmptyTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string EmptyDescription { get; set; } = string.Empty;
    }

    public class UploadedProductImageResponse
    {
        public string FileName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}