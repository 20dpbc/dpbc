using dpbc.entity.Entity;
using dpbc.repository.Repository;

namespace dpbc.service.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<User?> GetByUUID(string uuid)
        {
            var user = _unitOfWork.UserRepository.GetByUUID(uuid);
            return user;
        }

        public async Task<User> InsertAsync(User user)
        {
            await _unitOfWork.UserRepository.InsertAsync(user);
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            await _unitOfWork.UserRepository.UpdateAsync(user);
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            await _unitOfWork.UserRepository.DeleteAsync(user);
        }
    }
}
