using Device_Management_System_Backend.Models.Enums;

namespace Device_Management_System_Backend.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public DeviceType Type { get; set; }
        public DeviceOperatingSystem OS { get; set; }
        public string OSVersion { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public string RamAmount { get; set; } = string.Empty;
        public string? Description { get; set; }

        // FK
        public Guid? AssignedUserId { get; set; }
        // Navigation property
        public User? AssignedUser { get; set; }
    }
}
