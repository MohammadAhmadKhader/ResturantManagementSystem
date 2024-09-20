using Microsoft.AspNetCore.Mvc.Rendering;
using ResturantManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        [Display(Name = "Chef")]
        public int ChefId { get; set; }
        public string Status { get; set; } = null!;
        public Chef? Chef { get; set; }
        public List<OrderFood> OrdersFoodsList { get; set; } = new List<OrderFood>();

	}
}
