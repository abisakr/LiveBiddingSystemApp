using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Repositories.User.DTO
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
