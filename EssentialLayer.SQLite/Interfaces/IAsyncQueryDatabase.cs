using System.Collections.Generic;
using System.Threading.Tasks;

namespace EssentialLayer.SQLite.Interfaces
{
	public interface IAsyncQueryDatabase
	{
		Task<string> QueryAsync(string query, params object[] args);

		Task<IReadOnlyList<T>> QueryAsync<T>(string query, params object[] args) where T : class, new();

		Task<T?> QueryFirstsync<T>(string query, params object[] args) where T : class, new();
	}
}