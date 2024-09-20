using ResturantManagementSystem.Data;
using ResturantManagementSystem.Models;

namespace ResturantManagementSystem.Repositories
{
    public class IngredientsRepository : GenericRepository<Ingredient>
    {
        private readonly ApplicationDbContext context;
        public IngredientsRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
