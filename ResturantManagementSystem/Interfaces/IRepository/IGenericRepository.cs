using ResturantManagementSystem.Data;

namespace ResturantManagementSystem.Interfaces.IRepository
{
	public interface IGenericRepository<TModel>
	{
		public Task CreateAsync(TModel model);
        public void Create(TModel model);
        public Task CreateManyAsync(IEnumerable<TModel> models);
		public Task<TModel?> GetById(int Id);
		public Task UpdateById(int Id, Action<TModel> UpdateResource);
        public void UpdateByModel(TModel model);
        public Task<(int count, IEnumerable<TModel> list)> GetAll(int Page, int Limit, string? IncludedProperty = null);
		public Task<int> GetCount();
		public void Remove(TModel instance);
	}
}
