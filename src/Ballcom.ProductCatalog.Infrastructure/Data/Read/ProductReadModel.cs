namespace Ballcom.ProductCatalog.Infrastructure.Data.Read
{
    public class ProductReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal PriceAmount { get; set; }
        public string Currency { get; set; } = "";
        public int Stock { get; set; }
    }
}
