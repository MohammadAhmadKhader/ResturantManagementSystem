using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Models;

namespace ResturantManagementSystem.Repositories
{
	public class ChefsRepositry : GenericRepository<Chef>
	{
		private readonly ApplicationDbContext context;
		public ChefsRepositry(ApplicationDbContext context) : base(context)
		{
			this.context = context;
		}

		public async Task<Chef?> GetChefDetailsById(int Id)
		{
			var chefWithDetails = context.Chefs
				.Include(chef => chef.User)
				.FirstOrDefaultAsync(chef => chef.Id == Id);

			return await chefWithDetails;
        }
	}
}
