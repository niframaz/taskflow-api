using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserController> _logger = logger;
        // GET: api/<UserController>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationRequest user)
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
        public async Task<IActionResult> Login(UserLoginRequest user)
        {
            var token = await _userService.LoginAsync(user.Email, user.Password);
            if (token is not null)
            {
                return Ok(new { token });
            }
            _logger.LogWarning("User login failed for email: {Email}", user.Email);
            return Unauthorized();
        }
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(UserRoleCreationRequest request)
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
