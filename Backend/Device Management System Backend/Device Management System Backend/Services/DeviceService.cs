using AutoMapper;
using Device_Management_System_Backend.Helpers;
using Device_Management_System_Backend.Models;
using Device_Management_System_Backend.DTOs.Device;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using System.Text.RegularExpressions;

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


        public async Task<IEnumerable<DeviceResponse>> SearchDevicesAsync(string query)
        {
            var searchTokens = NormalizeQuery(query);

            if (!searchTokens.Any())
                return new List<DeviceResponse>();

            var allDevices = await _context.Devices.ToListAsync();

            // score, filter rank
            var rankedResults = allDevices
                .Select(device => new
                {
                    Device = device,
                    Score = CalculateRelevanceScore(device, searchTokens)
                })
                .Where(result => result.Score > 0)          
                .OrderByDescending(result => result.Score)  //sort desc by score
                .Select(result => result.Device)           
                .ToList();

            
            return rankedResults.Select(d => _mapper.Map<DeviceResponse>(d));
        }


        private List<string> NormalizeQuery(string input)
        {
            //turn query into lowercase / strip punctuation
            if (string.IsNullOrWhiteSpace(input)) return new List<string>();

            var lower = input.ToLowerInvariant();

            var clean = Regex.Replace(lower, @"[^\w\s]", "");

            //tokens by whitespace
            return clean.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private int CalculateRelevanceScore(Device device, List<string> searchTokens)
        {
            int score = 0;

            string nameClean = device.Name?.ToLowerInvariant() ?? "";
            string manufacturerClean = device.Manufacturer?.ToLowerInvariant() ?? "";
            string processorClean = device.Processor?.ToLowerInvariant() ?? "";
            string ramClean = device.RamAmount?.ToString().ToLowerInvariant() ?? "";

            foreach (var token in searchTokens)
            {
                //partial matching with contains
                if (nameClean.Contains(token)) score += 10;

                if (manufacturerClean.Contains(token)) score += 5;

                if (processorClean.Contains(token)) score += 3;

                if (ramClean.Contains(token)) score += 1;
            }

            return score;
        }
    }
}
