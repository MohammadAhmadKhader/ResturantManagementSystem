using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Models;
using System;

namespace ResturantManagementSystem.Repositories
{
    public class FoodsRepository : GenericRepository<Food>
    {
        private readonly ApplicationDbContext context;
        public FoodsRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<(int count, List<Food> list)> GetAllIncluded(int Page, int Limit)
        {
            var query = context.Foods
                .Include(f => f.FoodIngredients)
                .ThenInclude(fi => fi.Ingredient);

            var list = await query.OrderBy(x => x).Skip((Page - 1) * Limit).Take(Limit).ToListAsync();
            var count = await context.Foods.CountAsync();

            return (count, list);
        }

        public async Task<Food?> GetByIdIncluded(int Id)
        {
            var query = await context.Foods
                .Include(f => f.FoodIngredients)
                .ThenInclude(fi => fi.Ingredient)
                .FirstOrDefaultAsync(f => f.Id == Id);

            return query;
        }
        public async Task<List<Food>> GetFoodsByIdsAsync(List<int> foodIds)
        {
            var foods = await context.Foods
                               .Where(food => foodIds.Contains(food.Id))
                               .ToListAsync();

            return foods;
        }
    }
}
