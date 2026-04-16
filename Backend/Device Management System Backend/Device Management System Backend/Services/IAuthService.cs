using Device_Management_System_Backend.DTOs.Auth;

namespace Device_Management_System_Backend.Services
{

        public interface IAuthService
        {
            Task<bool> RegisterAsync(RegisterRequest request);
            Task<AuthResponse?> LoginAsync(LoginRequest request);
        }
}
