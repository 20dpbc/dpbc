using Dapper;
using dpbc.entity.Entity;
using dpbc.repository.Repository.Base;

namespace dpbc.repository.Repository
{
    public class PointRepository : Repository<Point>, IPointRepository
    {
        public PointRepository(IDBContext dbcontext) : base(dbcontext) { }

        public async Task<Point?> GetByUserAsync(long id)
        {
            var res = await _dbcontext.connection.QueryAsync<Point>(@"select * from Point where user_id=@id and stoped is null", param: new { id });
            return res.FirstOrDefault();
        }

        public async Task<PointView> GetTotalMinutes()
        {
            var res = await _dbcontext.connection.QueryAsync<(long user_id, int total_minutes)>(@"select user_id, sum(datepart(minute, stoped - started)) total_minutes from point where started > getdate()-6 and stoped is not null group by user_id");
            
            return new(res.Select(x => new Tuple<long, int>(x.user_id, x.total_minutes)).ToList());
        }
    }
}
