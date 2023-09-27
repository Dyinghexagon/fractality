using Microsoft.EntityFrameworkCore;

namespace Fractality.Repositories
{
    public class UserRepository : IDbRepository
    {
        private readonly DbContext _dbContext;

        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync<T>(T newEntity) where T : class
        {
            await _dbContext.AddAsync(newEntity);
            await _dbContext.SaveChangesAsync();
        }

        public Task AddRangeAsync<T>(IEnumerable<T> newEntities) where T : class
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllAsync<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync<T>(Guid id) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetT<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync<T>(Guid id) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
