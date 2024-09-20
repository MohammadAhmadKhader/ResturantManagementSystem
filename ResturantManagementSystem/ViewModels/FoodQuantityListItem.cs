using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class FoodQuantityListItem
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        [Range(1, 100, ErrorMessage = "Quantity can't be less than 1 or more than 100")]
        public int Quantity { get; set; }
    }
}
