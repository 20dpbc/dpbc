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

        public async Task<Point?> GetByUserIdAsync(long user_id)
        {
            var point = await _unitOfWork.PointRepository.GetByUserAsync(user_id);

            return point;
        }

        public async Task<Point> InsertAsync(Point point)
        {
            await _unitOfWork.PointRepository.InsertAsync(point);

            return point;
        }

        public async Task UpdateAsync(Point point)
        {
            await _unitOfWork.PointRepository.UpdateAsync(point);
        }

        public async Task DeleteAsync(Point point)
        {
            await _unitOfWork.PointRepository.DeleteAsync(point);
        }
    } 
}
