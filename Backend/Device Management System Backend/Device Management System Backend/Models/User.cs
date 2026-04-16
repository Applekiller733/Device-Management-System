namespace Device_Management_System_Backend.Models
{
    using System.Text.Json.Serialization;
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        //for auth:
        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
