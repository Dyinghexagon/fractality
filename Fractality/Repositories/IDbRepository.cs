namespace Fractality.Repositories
{
    public interface IDbRepository 
    {
        public IQueryable<T> GetAll<T>() where T : class;

        public IQueryable<T> GetT<T>() where T : class;

        public Task AddAsync<T>(T newEntity) where T : class;

        public Task AddRangeAsync<T>(IEnumerable<T> newEntities) where T : class;

        public Task DeleteAsync<T>(Guid id) where T : class;

        public Task DeleteAllAsync<T>() where T : class;

        public Task<T> UpdateAsync<T>(Guid id) where T : class;
    }
}
