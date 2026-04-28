using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerce.Models
{
    //model for product images
    public class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsMain { get; set; } = false;
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        // Navigation property
        [JsonIgnore]
        public Product Product { get; set; } = null!;
    }
}