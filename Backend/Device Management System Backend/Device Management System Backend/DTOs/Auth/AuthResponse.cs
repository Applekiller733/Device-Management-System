namespace Device_Management_System_Backend.DTOs.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
