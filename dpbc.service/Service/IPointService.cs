using Discord;
using dpbc.entity.Entity;

namespace dpbc.service.Service
{
    public interface IPointService
    {
        Task InsertAsync(ulong user_id, ulong message_id);
        Task<Point?> GetByUserIdAsync(ulong user_id);
        Task DeleteAsync(Point point);
    }
}