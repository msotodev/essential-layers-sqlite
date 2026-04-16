using System.Collections.Generic;

namespace EssentialLayer.SQLite.Interfaces
{
	public interface IQueryDatabaseService
	{
		string Query(string query, params object[] args);

		IReadOnlyList<T> Query<T>(string query, params object[] args) where T : class, new();

		T? QueryFirst<T>(string query, params object[] args) where T : class, new();
	}
}