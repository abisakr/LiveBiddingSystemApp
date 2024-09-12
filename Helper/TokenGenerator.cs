using Live_Bidding_System_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Live_Bidding_System_App.Helper
{
    public class TokenGenerator
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenGenerator(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(string Id)
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            );

            // Retrieve the user by Id and get their roles
            var user = await _userManager.FindByIdAsync(Id);  // Use UserManager to get the user
            var roles = await _userManager.GetRolesAsync(user);  // Get the roles for the user

            // Create the list of claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, Id),
        new Claim(ClaimTypes.Name, user.UserName)
    };

            // Add roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create the ClaimsIdentity
            var subject = new ClaimsIdentity(claims);

            var expires = DateTime.UtcNow.AddHours(3);

            // Create the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            // Generate and return the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }


    }
}
