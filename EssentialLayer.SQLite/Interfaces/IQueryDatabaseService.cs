using System.Collections.Generic;

namespace EssentialLayer.SQLite.Interfaces
{
	public interface IQueryDatabaseService
	{
		IReadOnlyList<T> Query<T>(string query, params object[] args) where T : class, new();

		T? QueryFirst<T>(string query, params object[] args) where T : class, new();

		int Version { get; }
	}
}