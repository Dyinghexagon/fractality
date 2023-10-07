using AutoMapper;
using Fractality.Models;
using Fractality.Models.Backend;
using Fractality.Models.Frontend;
using Fractality.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fractality.Services.UserServices
{
    public class UserServices : IUsersServices
    {
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public UserServices(
            IDbRepository userRepository,
            IMapper mapper
        ) 
        {
            _dbRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(UserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            var result = await _dbRepository.AddAsync(user);
            await _dbRepository.SaveChangesAsync();
            return result;
        }

        public async Task AddRangeAsync(IEnumerable<UserModel> userModels)
        {
            foreach (var userModel in userModels)
            {
                await AddAsync(userModel);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _dbRepository.DeleteAsync<User>(id);
            await _dbRepository.SaveChangesAsync();
        }

        public async Task<UserModel?> Get(Guid id)
        {
            var user = await _dbRepository.Get<User>(x => x.Id == id);
            var model = _mapper.Map<UserModel>(user);
            return model;
        }

        public async Task<IList<UserModel>> GetAll()
        {
            var result = await _dbRepository.GetAll<UserModel>();

            return result.ToList();
        }

        public async Task UpdateAsync(UserModel userModel, Guid id)
        {
            var user = _mapper.Map<User>(userModel);
            await _dbRepository.UpdateAsync(user, id);
            await _dbRepository.SaveChangesAsync();
        }
    }
}
