namespace ProSeeker.Data.Common
{
    using System;
    using System.Threading.Tasks;

    // Only if we want to use pure query (TSQL)
    public interface IDbQueryRunner : IDisposable
    {
        Task RunQueryAsync(string query, params object[] parameters);
    }
}
