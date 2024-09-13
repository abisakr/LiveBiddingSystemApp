using Google.Apis.Auth;
using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Models;
using Live_Bidding_System_App.Repositories.User;
using Live_Bidding_System_App.Repositories.User.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Live_Bidding_System_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserAccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public UserAccountController(UserManager<ApplicationUser> userManager, IUserAccountRepository userAccountRepository, IConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _userAccountRepository = userAccountRepository;
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest("Please enter the data.");

            var result = await _userAccountRepository.Login(loginDto);
            return result.IsSuccess
                ? Ok(new { Token = result.Data })
                : BadRequest(result.Message);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            if (registerDto == null)
                return BadRequest("Please enter the data.");

            var result = await _userAccountRepository.Register(registerDto);
            return result.IsSuccess
                ? Ok(new { Message = result.Message })
                : BadRequest(result.Message);
        }

        public class GoogleLoginRequestDto
        {
            public string Token { get; set; }
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Token))
                return BadRequest("Token is required.");

            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
                var user = await _userManager.FindByEmailAsync(payload.Email)
                    ?? new ApplicationUser { FullName = payload.Name, Email = payload.Email };

                if (user.Id == null)
                {
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                        return BadRequest("Failed to create user.");
                }

                var jwtToken = _tokenGenerator.GenerateToken(user.Id);
                return Ok(new { token = jwtToken, expiration = DateTime.UtcNow.AddHours(10) });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error validating Google token: {ex.Message}");
            }
        }
    }
}
