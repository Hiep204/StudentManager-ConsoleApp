namespace Webapp.DTO
{
    public class UpdateOrderStatusDTO
    {
        public int OrderId { get; set; }
        public string NewStatus { get; set; } = "";
        public int ChangedBy { get; set; }
        public string? Note { get; set; }
    }
}
