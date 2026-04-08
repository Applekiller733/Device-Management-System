using Device_Management_System_Backend.Models;

namespace Device_Management_System_Backend.Services
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAllDevices();
        Task<Device?> GetDeviceById(Guid id);
        Task<Device> CreateDevice(Device device);
        Task<bool> UpdateDevice(Device device);
        Task<bool> DeleteDevice(Guid id);
    }
}
