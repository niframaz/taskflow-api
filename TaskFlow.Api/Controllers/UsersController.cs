using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UsersController> _logger = logger;
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest user)
        {
            var appUser = _mapper.Map<ApplicationUser>(user);
            var token = await _userService.RegisterAsync(appUser, user.Password);
            if (token is not null)
            {
                return Ok(new { token });
            }
            _logger.LogWarning("User registration failed for email: {Email}", user.Email);
            return StatusCode(500);
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto?>> Login(LoginRequest user)
        {
            var response = await _userService.LoginAsync(user.Email, user.Password);
            if (response is not null)
            {
                return Ok(response);
            }
            _logger.LogWarning("User login failed for email: {Email}", user.Email);
            return Unauthorized();
        }
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(UserRoleRequest request)
        {
            var result = await _userService.CreateRoleAsync(request.Role);
            if (result)
            {
                return Ok();
            }
            _logger.LogWarning("Role creation failed for role: {Role}", request.Role.ToString());
            return StatusCode(500);
        }
    }
}
