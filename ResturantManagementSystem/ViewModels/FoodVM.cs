using Microsoft.AspNetCore.Mvc.Rendering;
using ResturantManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class FoodVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public List<Ingredient>? Ingredients;
    }
}
