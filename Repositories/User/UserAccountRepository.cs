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
        public UserAccountRepository(
      UserManager<ApplicationUser> userManager, TokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<string> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Email);
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (user != null && passwordCheck)
                {
                    var token = await _tokenGenerator.GenerateToken(user.Id);
                    return token;
                }
                return string.Empty;
            }

            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching user.", ex);
            }
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            try
            {
                // Create a new ApplicationUser object
                var user = new ApplicationUser
                {
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNo,
                    Address = registerDto.Address,
                    UserName = registerDto.FullName,
                };

                // Create the user in the database
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    // Assign the "Buyer" role to the newly created user
                    var roleResult = await _userManager.AddToRoleAsync(user, "Buyer");

                    if (roleResult.Succeeded)
                    {
                        return "User registered successfully and assigned to Buyer role.";
                    }
                    else
                    {
                        return "User registered successfully but role assignment failed.";
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering user.", ex);
            }
        }

    }
}
