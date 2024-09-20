using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using ResturantManagementSystem.Data;

namespace ResturantManagementSystem.Repositories
{
    public class UnitOfWork
    {
        public ApplicationDbContext appDbContext;
        public ChefsRepositry chefsRepository { get; set; }
        public IngredientsRepository ingredientsRepository { get; set; }
        public FoodsRepository foodsRepository { get; set; }
        public OrdersRepository ordersRepository { get; set; }
        public FoodsIngredientsRepository foodsIngredientsRepository { get; set; }
        public OrdersFoodsRepository ordersFoodsRepository { get; set; }
        public UnitOfWork(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.chefsRepository = new ChefsRepositry(appDbContext);
            this.ingredientsRepository = new IngredientsRepository(appDbContext);
            this.foodsRepository = new FoodsRepository(appDbContext);
            this.ordersRepository = new OrdersRepository(appDbContext);
            this.foodsIngredientsRepository = new FoodsIngredientsRepository(appDbContext);
            this.ordersFoodsRepository = new OrdersFoodsRepository(appDbContext);
        }
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<IDbContextTransaction> StartTransactionAsync()
        {
            var transaction = await appDbContext.Database.BeginTransactionAsync();
            return transaction;
        }
    }
}
