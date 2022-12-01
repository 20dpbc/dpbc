namespace dpbc.repository.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IPointRepository PointRepository { get; }
    }
}
