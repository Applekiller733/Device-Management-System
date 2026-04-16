namespace Device_Management_System_Backend.Services
{
    public interface IAIDescriptionService
    {
        Task<string> GenerateDeviceDescriptionAsync(string name, string manufacturer, string type, string os, string ram, string processor);
    }
}
