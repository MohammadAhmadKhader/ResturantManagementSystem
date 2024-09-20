using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.Models
{
    public class Food
    {
        public int Id { get; set; }
        [Required]
        [MinLength(4, ErrorMessage ="Minimum characters allowed is 4.")]
        [MaxLength(64, ErrorMessage = "Maximum characters allowed is 64.")]
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public ICollection<FoodIngredient> FoodIngredients { get; set; } = new HashSet<FoodIngredient>();
        public ICollection<OrderFood> OrderFoods { get; set; } = new HashSet<OrderFood>();
    }
}
