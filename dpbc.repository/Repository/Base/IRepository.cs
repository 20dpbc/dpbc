namespace dpbc.repository.Repository.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity entity);
        Task InsertListAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> GetAsync(long id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task UpdateAsync(TEntity entity);
    }
}
