using Fractality.Models;
using System.Linq.Expressions;

namespace Fractality.Repositories
{
    public interface IDbRepository
    {
        public Task<IQueryable<T>> GetAll<T>() where T : class, IEntity;

        public Task<IQueryable<T>> Get<T>(Expression<Func<T, bool>> selector) where T : class, IEntity;

        public Task<Guid> AddAsync<T>(T newEntity) where T : class, IEntity;

        public Task AddRangeAsync<T>(IEnumerable<T> newEntities) where T : class, IEntity;

        public Task DeleteAsync<T>(Guid id) where T : class, IEntity;

        public Task UpdateAsync<T>(T newEntity, Guid id) where T : class, IEntity;

        public Task<int> SaveChangesAsync();
    }
}
