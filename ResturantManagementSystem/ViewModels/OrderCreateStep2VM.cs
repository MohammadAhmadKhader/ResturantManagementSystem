using ResturantManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class OrderCreateStep2VM
    {
        public int ChefId { get; set; }
        public Chef? Chef { get; set; }
        public List<FoodQuantityListItem> Selections { get; set; } = new List<FoodQuantityListItem>();
    }
}
