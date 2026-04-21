namespace Webapp.DTO
{
    public class ProductDetailDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = "";
        public string? SupplierName { get; set; }
        public int UnitsInStock { get; set; }
        public string? Description { get; set; }
    }
}
