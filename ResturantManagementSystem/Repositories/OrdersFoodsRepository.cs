using ResturantManagementSystem.Data;
using ResturantManagementSystem.Models;

namespace ResturantManagementSystem.Repositories
{
    public class OrdersFoodsRepository : GenericRepository<OrderFood>
    {
        private readonly ApplicationDbContext context;
        public OrdersFoodsRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
