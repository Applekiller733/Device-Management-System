using Device_Management_System_Backend.Models;
using Device_Management_System_Backend.DTOs.Device;

namespace Device_Management_System_Backend.Services
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceResponse>> GetAllDevicesAsync();
        Task<DeviceResponse?> GetDeviceByIdAsync(Guid id);
        Task<DeviceResponse> CreateDeviceAsync(CreateDeviceRequest request);
        Task<bool> UpdateDeviceAsync(UpdateDeviceRequest request);
        Task<bool> DeleteDeviceAsync(Guid id);
        Task<bool> AssignDeviceAsync(Guid deviceId, Guid userId);
        Task<bool> UnassignDeviceAsync(Guid deviceId, Guid userId);
        Task<IEnumerable<DeviceResponse>> SearchDevicesAsync(string query);
    }
}
