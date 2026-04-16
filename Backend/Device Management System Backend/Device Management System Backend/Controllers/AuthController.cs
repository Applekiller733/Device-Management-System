using Device_Management_System_Backend.DTOs.Auth;
using Device_Management_System_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Device_Management_System_Backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Device_Management_System_Backend.Services;

namespace Device_Management_System_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var success = await _authService.RegisterAsync(request);
            return success ? Ok() : BadRequest("User already exists.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return response != null ? Ok(response) : Unauthorized("Invalid credentials.");
        }
    }
}
