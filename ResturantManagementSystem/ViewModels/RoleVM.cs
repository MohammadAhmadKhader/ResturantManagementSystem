using System.ComponentModel.DataAnnotations;

namespace ResturantManagementSystem.ViewModels
{
    public class RoleVM
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(4,ErrorMessage ="Name must be at least 4 characters")]
        [MaxLength(100, ErrorMessage = "Name length can not exceed 100 characters")]
        public string Name { get; set; } = null!;
    }
}
