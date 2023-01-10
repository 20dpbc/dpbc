using System.Data;

namespace dpbc.repository.Repository
{
    public interface IDBContext : IDisposable
    {
        IDbConnection connection { get; }
    }
}
