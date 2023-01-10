using Dapper;
using dpbc.entity.Entity;

namespace dpbc.repository.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDBContext dbcontext) : base(dbcontext)
        {
        }

        public async Task<User?> GetByUUID(string uuid)
        {
            var ret = await _dbcontext.connection.QueryAsync<User>(@"select * from [User] where uuid=@uuid", param: new { uuid });
            return ret.FirstOrDefault();
        }
    }
}
