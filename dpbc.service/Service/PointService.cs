using dpbc.entity.Entity;
using dpbc.repository.Repository.Base;

namespace dpbc.service.Service
{
    public class PointService : IPointService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PointService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Point?> GetByUserIdAsync(ulong user_id)
        {
            var point = await _unitOfWork.PointRepository.GetByUserAsync(user_id);

            return point;
        }

        public async Task InsertAsync(ulong user_id, ulong message_id)
        {
            await _unitOfWork.PointRepository.InsertAsync(new(user_id, message_id));
        }

        public async Task DeleteAsync(Point point)
        {
            await _unitOfWork.PointRepository.DeleteAsync(point);
        }
    } 
}
