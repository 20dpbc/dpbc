using Dapper;
using dpbc.entity.Entity;

namespace dpbc.repository.Repository
{
    public class PointRepository : Repository<Point>, IPointRepository
    {
        public PointRepository(IDBContext dbcontext) : base(dbcontext) { }

        public async Task<Point?> GetByUserAsync(long user_id)
        {
            var res = await _dbcontext.connection.QueryAsync<Point>(@"select * from Point where user_id=@user_id and stoped is null", param: new { user_id });
            return res.FirstOrDefault();
        }

        public async Task<PointView> GetTotalMinutes(int days)
        {
            string sql = @"
            select 
	            u.mention, 
	            iif(sum(datediff(minute, p.started, p.stoped)) is null, 0, sum(datediff(minute, p.started, p.stoped))) total_minutes 
            from 
	            [User] u 
            left join 
	            Point p on u.id = p.user_id 
            where
	            p.started > getdate() - @days or p.started is null
            group by u.mention";

            var res = await _dbcontext.connection.QueryAsync<(string mention, int total_minutes)>(sql, param: new { days });            
            return new(res.Select(x => new Tuple<string, int>(x.mention, x.total_minutes)).ToList());
        }
    }
}
