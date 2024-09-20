using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        [MinLength(4, ErrorMessage = "Minimum characters allowed is 4.")]
        [MaxLength(64, ErrorMessage = "Maximum characters allowed is 64.")]
        public string Name { get; set; } = null!;
        [MinLength(10, ErrorMessage = "Minimum characters allowed is 10.")]
        [MaxLength(1024, ErrorMessage = "Maximum characters allowed is 1024.")]
        public string Description { get; set; } = null!;
        public ICollection<FoodIngredient> FoodIngredients { get; set; } = new HashSet<FoodIngredient>();
    }
}
