using Discord;
using dpbc.entity.Entity;

namespace dpbc.service.Service
{
    public interface IPointService
    {
        Task<Point> InsertAsync(Point point);
        Task<Point?> GetByUserIdAsync(long user_id);
        Task<PointView> GetTotalMinutes(int days);
        Task UpdateAsync(Point point);
        Task DeleteAsync(Point point);
    }
}