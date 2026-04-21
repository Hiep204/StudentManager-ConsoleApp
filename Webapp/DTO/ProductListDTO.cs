namespace Webapp.DTO
{
    public class ProductListDTO
    {
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public string? ImageUrl { get; set; }
    public string CategoryName { get; set; } = "";

    }
}
