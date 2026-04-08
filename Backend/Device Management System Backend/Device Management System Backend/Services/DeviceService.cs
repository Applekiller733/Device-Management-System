using Device_Management_System_Backend.Helpers;
using Device_Management_System_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Device_Management_System_Backend.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly DataContext _context;

        public DeviceService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            return await _context.Devices
                .Include(d => d.AssignedUser)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Device?> GetDeviceById(Guid id)
        {
            return await _context.Devices
                .Include(d => d.AssignedUser)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Device> CreateDevice(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<bool> UpdateDevice(Device device)
        {
            _context.Entry(device).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteDevice(Guid id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return false;

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
