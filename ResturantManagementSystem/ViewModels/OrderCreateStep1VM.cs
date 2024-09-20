using Microsoft.AspNetCore.Mvc.Rendering;
using ResturantManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class OrderCreateStep1VM
    {
        public int Id { get; set; }
        [Display(Name = "Chef")]
        public int ChefId { get; set; }
        public List<Food>? Select { get; set; }
        public List<int> SelectedFoods { get; set; } = new List<int>();
        public List<SelectListItem>? Foods { get; set; }
        public List<SelectListItem>? Chefs { get; set; }
    }
}
