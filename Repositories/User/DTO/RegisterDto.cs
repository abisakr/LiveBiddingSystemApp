using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Repositories.User.DTO
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string? Address { get; set; }
        public IFormFile? Photo { get; set; }

        [Required]
        public string Password { get; set; }
        public string? Token { get; set; }

    }
}
