using System.ComponentModel.DataAnnotations;

namespace Ballcom.ProductCatalog.WebAPI.Models
{
    public class CreateProductModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal PriceAmount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = "EUR";

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
