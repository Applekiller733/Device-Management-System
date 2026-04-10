using Device_Management_System_Backend.DTOs.User;

namespace Device_Management_System_Backend.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<bool> UpdateUserAsync(UpdateUserRequest request);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
