namespace Webapp.DTO
{
    public class SaveAddressDTO
    {
        public int UserId { get; set; }
        public string ReceiverName { get; set; } = "";
        public string ReceiverPhone { get; set; } = "";
        public string AddressLine { get; set; } = "";
        public string Ward { get; set; } = "";
        public string District { get; set; } = "";
        public string City { get; set; } = "";
        public bool IsDefault { get; set; }
    }
}
