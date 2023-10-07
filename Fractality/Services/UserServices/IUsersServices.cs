using Fractality.Models;
using Fractality.Models.Backend;
using Fractality.Models.Frontend;

namespace Fractality.Services.UserServices
{
    public interface IUsersServices : IServices<UserModel>
    {
        public Task<Guid> AddAsync(UserModel newEntity);
    }
}
