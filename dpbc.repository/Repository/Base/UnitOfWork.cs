namespace dpbc.repository.Repository.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDBContext _dbcontext;

        public UnitOfWork(IDBContext dbcontext) => _dbcontext = dbcontext;

        private IPointRepository _pointRepository;
        public IPointRepository PointRepository => GetInstance<IPointRepository, PointRepository>(ref _pointRepository, _dbcontext);

        public void Dispose() => _dbcontext.Dispose();

        private static T GetInstance<T, U>(ref T repository, IDBContext dbcontext)
        {
            if (repository == null)
                repository = (T)Activator.CreateInstance(typeof(U), dbcontext);

            return repository;
        }
    }
}
