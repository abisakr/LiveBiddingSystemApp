using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Models;
using Live_Bidding_System_App.Repositories.User.DTO;
using Microsoft.AspNetCore.Identity;

namespace Live_Bidding_System_App.Repositories.User
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;

        public UserAccountRepository(UserManager<ApplicationUser> userManager, TokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<OperationResult<string>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    return OperationResult<string>.FailureResult("Invalid username or password.");
                }

                var token = await _tokenGenerator.GenerateToken(user.Id);
                return OperationResult<string>.SuccessResult("Login successful.", token);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult("Error occurred while logging in: " + ex.Message);
            }
        }

        public async Task<OperationResult<string>> Register(RegisterDto registerDto)
        {
            try
            {
                // Check if the email is already in use
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return OperationResult<string>.FailureResult("Email is already in use.");
                }

                // Create a new ApplicationUser object
                var user = new ApplicationUser
                {
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNo,
                    Address = registerDto.Address,
                    FullName = registerDto.FullName,
                    UserName = registerDto.FullName.Split(' ')[0].ToLower()
                };

                // Create the user in the database
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    return OperationResult<string>.FailureResult("User registration failed.");
                }

                // Assign the "Buyer" role to the newly created user
                var roleResult = await _userManager.AddToRoleAsync(user, "Buyer");
                return roleResult.Succeeded
                    ? OperationResult<string>.SuccessResult("User registered successfully and assigned to Buyer role.")
                    : OperationResult<string>.FailureResult("User registered successfully but role assignment failed.");
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult("Error occurred while registering user: " + ex.Message);
            }
        }

    }
}
