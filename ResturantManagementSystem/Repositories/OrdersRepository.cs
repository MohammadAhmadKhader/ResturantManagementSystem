using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Models;

namespace ResturantManagementSystem.Repositories
{
    public class OrdersRepository : GenericRepository<Order>
    {
        private readonly ApplicationDbContext context;
        public OrdersRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<Order?> GetOrderByIdAllIncluded(int Id)
        {
            var orderWithDetails = context.Orders
                .Include(order => order.OrderFoods)
                .ThenInclude(of => of.Food)
                .Include(order => order.Chef)
                .FirstOrDefaultAsync(order => order.Id == Id);

            return await orderWithDetails;
        }

        public async Task<(int count, List<Order> list)> GetAllIncluded(int Page, int Limit)
        {
            var query = context.Orders
                .Include(o => o.OrderFoods)
                .ThenInclude(o => o.Food)
                .Include(o => o.Chef)
                .ThenInclude(c => c.User);

            var list = await query.OrderBy(x => x).Skip((Page - 1) * Limit).Take(Limit).ToListAsync();
            var count = await context.Foods.CountAsync();

            return (count, list);
        }
    }
}
