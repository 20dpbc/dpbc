using dpbc.entity.Entity;
using dpbc.repository.Repository.Base;

namespace dpbc.repository.Repository
{
    public interface IPointRepository : IRepository<Point>
    {
        Task<Point?> GetByUserAsync(long id);
    }
}