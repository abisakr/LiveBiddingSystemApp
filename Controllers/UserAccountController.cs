﻿using Google.Apis.Auth;
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
            try
            {
                if (loginDto != null)
                {
                    var result = await _userAccountRepository.Login(loginDto);
                    if (string.IsNullOrEmpty(result))
                    {
                        return BadRequest("Invalid username or password.");
                    }
                    return Ok(new { Token = result });
                }
                else return BadRequest("Please Enter The Data");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request." + ex);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            try
            {
                if (registerDto != null)
                {
                    var result = await _userAccountRepository.Register(registerDto);
                    if (string.IsNullOrEmpty(result))
                    {
                        return BadRequest("Registeration Failed.");
                    }

                    return Ok(new { Message = result });

                }
                else return BadRequest("Please Enter The Data");
            }

            catch (Exception)
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public class GoogleLoginRequestDto
        {
            public string Token { get; set; }
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
                var user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = payload.Name,
                        Email = payload.Email
                    };
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        return BadRequest("Failed to create user.");
                    }
                }

                // Use the GenerateToken method to create the JWT token
                var jwtToken = _tokenGenerator.GenerateToken(user.Id);

                return Ok(new
                {
                    token = jwtToken,
                    expiration = DateTime.UtcNow.AddHours(10) // Ensure this matches the expiration in GenerateToken
                });
            }
            catch (Exception ex)
            {
                // Log exception
                return BadRequest($"Error validating Google token: {ex.Message}");
            }
        }

    }
}
