using dpbc.entity.Entity;

namespace dpbc.repository.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUUID(string uuid);
    }
}