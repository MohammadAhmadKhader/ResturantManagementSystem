using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResturantManagementSystem.Models;

namespace ResturantManagementSystem.ViewModels
{
	public class RmsUserUpdateRolesVM
	{
		public string Id { get; set; } = null!;
		public string? UserName { get; set; }
		public string? Email { get; set; }
		public List<string>? SelectedRoles { get; set; }
		public List<SelectListItem>? Roles { get; set; }
	}
}
