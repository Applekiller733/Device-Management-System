namespace Device_Management_System_Backend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
