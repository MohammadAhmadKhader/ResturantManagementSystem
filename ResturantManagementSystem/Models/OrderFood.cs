using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantManagementSystem.Models
{
    public class OrderFood
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        [Required]
        [ForeignKey(nameof(Food))]
        public int FoodId { get; set; }
        public Food? Food { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "The quantity must be between 0 and 100.")]
        public int Quantity { get; set; }
    }
}
