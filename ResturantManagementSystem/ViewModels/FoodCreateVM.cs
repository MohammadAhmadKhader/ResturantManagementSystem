using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class FoodCreateVM
    {
        public int Id { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Minimum characters allowed is 4.")]
        [MaxLength(64, ErrorMessage = "Maximum characters allowed is 64.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage ="Image is required")]
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl{ get; set; }
        public List<int> SelectedIngredients { get; set; } = new List<int>();
        public List<SelectListItem>? Ingredients { get; set; }
    }
}
