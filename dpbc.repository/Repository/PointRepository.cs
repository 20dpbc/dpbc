using Dapper;
using dpbc.entity.Entity;
using dpbc.repository.Repository.Base;

namespace dpbc.repository.Repository
{
    public class PointRepository : Repository<Point>, IPointRepository
    {
        public PointRepository(IDBContext dbcontext) : base(dbcontext) { }

        public async Task<Point?> GetByUserAsync(ulong id)
        {
            var res = await _dbcontext.connection.QueryAsync<Point>(@"select * from Point where user_id=@id", param: new { id });
            return res.FirstOrDefault();
        }
    }
}
