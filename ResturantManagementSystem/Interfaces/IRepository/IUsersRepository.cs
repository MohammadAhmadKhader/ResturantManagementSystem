using Microsoft.AspNetCore.Identity;
using ResturantManagementSystem.Repositories;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Interfaces.IRepository
{
	public interface IUsersRepository : IGenericRepository<IdentityUser>
	{
		public bool IsUniqueUser(string Email);
	}
}
