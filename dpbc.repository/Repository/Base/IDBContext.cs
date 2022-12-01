using System.Data;

namespace dpbc.repository.Repository.Base
{
    public interface IDBContext : IDisposable
    {
        IDbConnection connection { get; }
    }
}
