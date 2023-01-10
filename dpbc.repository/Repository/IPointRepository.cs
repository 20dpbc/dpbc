using dpbc.entity.Entity;

namespace dpbc.repository.Repository
{
    public interface IPointRepository : IRepository<Point>
    {
        Task<Point?> GetByUserAsync(long id);
        Task<PointView> GetTotalMinutes(int days);
    }
}