using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class ChefCreateVM
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Range(0.00, 5000.00, ErrorMessage = "Salary can be any value between 0.00 and 5000.00")]
        public double Salary { get; set; }
    }
}
