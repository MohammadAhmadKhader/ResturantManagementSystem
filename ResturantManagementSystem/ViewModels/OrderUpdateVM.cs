using Microsoft.AspNetCore.Mvc.Rendering;
using ResturantManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class OrderUpdateVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Status is required")]
        public string Status { get; set; } = null!;
        public List<FoodQuantityListItem> Selections { get; set; } = new List<FoodQuantityListItem>();
        public Chef? Chef { get; set; }
        public int ChefId { get; set; }
        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Completed", Value = "Completed" },
            new SelectListItem { Text = "Pending", Value = "Pending" },
            new SelectListItem { Text = "Cancelled", Value = "Cancelled" },
            new SelectListItem { Text = "Rejected", Value = "Rejected" },
        };
    }
}
