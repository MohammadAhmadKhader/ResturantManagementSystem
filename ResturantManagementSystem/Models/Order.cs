using ResturantManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantManagementSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Chef))]
        public int ChefId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Chef? Chef { get; set; }
        public ICollection<OrderFood> OrderFoods { get; set; } = new HashSet<OrderFood>();
    }
}
