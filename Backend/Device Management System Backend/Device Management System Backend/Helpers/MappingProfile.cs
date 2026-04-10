
using AutoMapper;

namespace Device_Management_System_Backend.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //mappings for user
            CreateMap<Models.User, DTOs.User.UserResponse>();
            CreateMap<DTOs.User.CreateUserRequest, Models.User>();
            CreateMap<DTOs.User.UpdateUserRequest, Models.User>();
            //mappings for device
            CreateMap<Models.Device, DTOs.Device.DeviceResponse>();
            CreateMap<DTOs.Device.CreateDeviceRequest, Models.Device>();
            CreateMap<DTOs.Device.UpdateDeviceRequest, Models.Device>();
        }
    }
}
