using dpbc.entity.Entity;

namespace dpbc.service.Service
{
    public interface IUserService
    {
        Task<User?> GetByUUID(string uuid);
        Task<User> InsertAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
