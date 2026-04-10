using AutoMapper;
using Device_Management_System_Backend.Helpers;
using Device_Management_System_Backend.Models;
using Device_Management_System_Backend.DTOs.Device;
using Microsoft.EntityFrameworkCore;
using Azure.Core;

namespace Device_Management_System_Backend.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeviceService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeviceResponse>> GetAllDevicesAsync()
        {
            var devices = await _context.Devices
                    .Include(d => d.AssignedUser)
                    .AsNoTracking()
                    .ToListAsync();
            return _mapper.Map<IEnumerable<DeviceResponse>>(devices);
        }

        public async Task<DeviceResponse?> GetDeviceByIdAsync(Guid id)
        {
            var device =  await _context.Devices
                .Include(d => d.AssignedUser)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (device == null) 
                return null;
            return _mapper.Map<DeviceResponse>(device);
        }

        public async Task<DeviceResponse> CreateDeviceAsync(CreateDeviceRequest request)
        {
            var device = _mapper.Map<Device>(request);

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return _mapper.Map<DeviceResponse>(device);
        }

        public async Task<bool> UpdateDeviceAsync(UpdateDeviceRequest request)
        {
            var existingDevice = await _context.Devices.FindAsync(request.Id);

            if (existingDevice == null) 
                throw new KeyNotFoundException($"Device with ID {request.Id} not found.");

            _mapper.Map(request, existingDevice);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDeviceAsync(Guid id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return false;

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
