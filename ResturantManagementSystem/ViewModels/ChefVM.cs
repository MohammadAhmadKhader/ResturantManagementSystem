using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class ChefVM
    {
        public int Id { get; set; }
        public string ChefName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public double Salary { get; set; }
    }
}
