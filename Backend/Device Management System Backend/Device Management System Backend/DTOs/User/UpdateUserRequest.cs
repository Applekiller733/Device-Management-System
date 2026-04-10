namespace Device_Management_System_Backend.DTOs.User
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
