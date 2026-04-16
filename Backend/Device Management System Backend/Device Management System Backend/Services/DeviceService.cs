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
        private readonly IAIDescriptionService _aidescriptionservice;

        public DeviceService(DataContext context, IMapper mapper, IAIDescriptionService aidescriptionservice)
        {
            _context = context;
            _mapper = mapper;
            _aidescriptionservice = aidescriptionservice;
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

            var description = request.Description;

            if (string.IsNullOrWhiteSpace(description))
            {
                description = await _aidescriptionservice.GenerateDeviceDescriptionAsync(
                    request.Name, request.Manufacturer, request.Type.ToString(),
                    request.OS.ToString(), request.RamAmount, request.Processor);
            }

            device.Description = description;

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

        public async Task<bool> AssignDeviceAsync(Guid deviceId, Guid userId)
        {
            var device = await _context.Devices.FindAsync(deviceId);
            if (device == null || device.AssignedUserId != null) return false;

            device.AssignedUserId = userId;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnassignDeviceAsync(Guid deviceId, Guid userId)
        {
            var device = await _context.Devices.FindAsync(deviceId);
            
            if (device == null || device.AssignedUserId != userId) return false;

            device.AssignedUserId = null;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
