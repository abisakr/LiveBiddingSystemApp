using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Repositories.Seller.DTO
{
    public class CreateItemCategoryDto
    {
        [Required]
        public string CategoryName { get; set; }
    }
}
