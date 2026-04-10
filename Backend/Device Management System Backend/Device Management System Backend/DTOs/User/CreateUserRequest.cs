using System.Text.Json.Serialization;

namespace Device_Management_System_Backend.DTOs.User
{
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
