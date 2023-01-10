namespace dpbc.repository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IPointRepository PointRepository { get; }
        IUserRepository UserRepository { get; }
    }
}
