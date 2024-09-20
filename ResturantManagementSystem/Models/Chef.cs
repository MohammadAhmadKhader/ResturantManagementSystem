using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantManagementSystem.Models
{
    public class Chef
    {
        public int Id { get; set; }
        [Required]
        [Range(0.00, 5000.00, ErrorMessage = "Salary can be any value between 0.00 and 5000.00")]
        public double Salary { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public IdentityUser? User { get; set; }
    }
}
