using AutoMapper;
using Device_Management_System_Backend.Helpers;
using Device_Management_System_Backend.Models;
using Device_Management_System_Backend.DTOs.User;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.EntityFrameworkCore;


namespace Device_Management_System_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            var user =  await _context.Users
                .Include(u => u.Devices) 
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return null;
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<bool> UpdateUserAsync(UpdateUserRequest request)
        {
            var existingUser = await _context.Users.FindAsync(request.Id);

            if (existingUser == null) 
                throw new KeyNotFoundException($"Device with ID {request.Id} not found."); ;

            _mapper.Map(request, existingUser);

            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
