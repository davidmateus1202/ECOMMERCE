using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class StorefrontSection
    {
        [Key]
        public int StorefrontSectionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;

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

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}