using Device_Management_System_Backend.DTOs.Device;
using Device_Management_System_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Device_Management_System_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceResponse>> GetDevice(Guid id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null) return NotFound();
            return Ok(device);
        }
        
        [HttpPost]
        public async Task<ActionResult<DeviceResponse>> CreateDevice(CreateDeviceRequest device)
        {
            var createdDevice = await _deviceService.CreateDeviceAsync(device);
            return CreatedAtAction(nameof(GetDevice), new { id = createdDevice.Id }, createdDevice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(Guid id, UpdateDeviceRequest device)
        {
            if (id != device.Id) return BadRequest("ID mismatch");

            var success = await _deviceService.UpdateDeviceAsync(device);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(Guid id)
        {
            var success = await _deviceService.DeleteDeviceAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> Assign(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var success = await _deviceService.AssignDeviceAsync(id, userId);
            return success ? Ok() : BadRequest("Could not assign device.");
        }

        [HttpPost("{id}/unassign")]
        public async Task<IActionResult> Unassign(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var success = await _deviceService.UnassignDeviceAsync(id, userId);
            return success ? Ok() : BadRequest("Could not unassign device.");
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DeviceResponse>>> SearchDevices([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var devices = await _deviceService.SearchDevicesAsync(q);
            return Ok(devices);
        }
    }
}
