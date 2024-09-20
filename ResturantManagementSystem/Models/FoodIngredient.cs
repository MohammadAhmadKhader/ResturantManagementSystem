using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantManagementSystem.Models
{
    public class FoodIngredient
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Food))]
        public int FoodId { get; set; }
        public Food? Food { get; set; }
        [Required]
        [ForeignKey(nameof(Ingredient))]
        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
