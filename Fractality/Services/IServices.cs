using Fractality.Models;

namespace Fractality.Services
{
    public interface IServices<T> where T : class, IEntity
    {
        public Task<T?> Get(Guid id);

        public Task<IList<T>> GetAll();

        public Task AddRangeAsync(IEnumerable<T> newEntities);

        public Task DeleteAsync(Guid id);

        public Task UpdateAsync(T newEntity, Guid id);
    }
}
