namespace Webapp.DTO
{
    public class CheckoutDTO
    {
        public int UserId { get; set; }
        public string ReceiverName { get; set; } = "";
        public string ReceiverPhone { get; set; } = "";
        public string ShippingAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string? Note { get; set; }
    }
}
