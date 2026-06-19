using System.ComponentModel.DataAnnotations;

namespace Ballcom.ProductCatalog.Domain.Domain
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public required int StockQuantity { get; set; }

    }
}
