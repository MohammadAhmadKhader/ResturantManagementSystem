using ResturantManagementSystem.Data;
using ResturantManagementSystem.Models;

namespace ResturantManagementSystem.Repositories
{
    public class FoodsIngredientsRepository : GenericRepository<FoodIngredient>
    {
        private readonly ApplicationDbContext context;
        public FoodsIngredientsRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
