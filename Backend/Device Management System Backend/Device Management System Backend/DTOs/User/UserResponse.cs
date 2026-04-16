using System.Text.Json.Serialization;
using Device_Management_System_Backend.DTOs.Device;

namespace Device_Management_System_Backend.DTOs.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<DeviceResponse> Devices { get; set; } = new List<DeviceResponse>();
    }
}
