using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Interfaces.IRepository;
using System.Runtime.InteropServices;

namespace ResturantManagementSystem.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class 
    {
        private readonly ApplicationDbContext appDbContext;
        public GenericRepository(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public void Create(TModel model)
        {
            appDbContext.Set<TModel>().Add(model);
        }

        public async Task CreateAsync(TModel model)
        {
            await appDbContext.Set<TModel>().AddAsync(model);
        }
        public async Task CreateManyAsync(IEnumerable<TModel> models)
        {
            await appDbContext.Set<TModel>().AddRangeAsync(models);
        }
        public async Task<TModel?> GetById(int Id)
        {
            var resource = await appDbContext.Set<TModel>().FindAsync(Id);
            return resource;
        }
        public async Task UpdateById(int Id, Action<TModel> UpdateResource)
        {
            try
            {
                var resource = await appDbContext.Set<TModel>().FindAsync(Id);
                if(resource == null)
                {
                    throw new ArgumentException($"{typeof(TModel)} was not found");
                }

                UpdateResource(resource);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void UpdateByModel(TModel model)
        {
           appDbContext.Set<TModel>().Update(model);
        }

        public async Task<bool> Delete(int Id)
        {
            var model = await appDbContext.Set<TModel>().FindAsync(Id);
            if(model == null)
            {
                return false;
            }
            appDbContext.Set<TModel>().Remove(model);
            return true;
        }

        public async Task<(int count, IEnumerable<TModel> list)> GetAll(int Page, int Limit, string? IncludedProperty = null)
        {
            var query = appDbContext.Set<TModel>().AsQueryable();

            if (IncludedProperty != null)
            {
                query = query.Include(IncludedProperty);
            }

            var list = await query.OrderBy(x => x).Skip((Page - 1) * Limit).Take(Limit).ToListAsync();
            var count = await appDbContext.Set<TModel>().CountAsync();

            return (count, list);
        }

        public async Task<int> GetCount()
        {
            var count = await appDbContext.Set<TModel>().CountAsync();
            return count;
        }

        public void Remove(TModel instance)
        {
            appDbContext.Set<TModel>().Remove(instance);
        }
    }
}
