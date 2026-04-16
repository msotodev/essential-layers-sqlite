using EssentialLayers.Helpers.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EssentialLayer.SQLite.Interfaces
{
	public interface IAsyncDatabaseService
	{
		Task<Response> CreateAsync<T>() where T : new();

		Task<Response> DeleteAsync<T>(T data) where T : new();

		Task<Response> DeleteAllAsync<T>();

		Task<Response> DropAsync<T>() where T : new();

		Task<Response> ExecuteAsync(string script, params object[] args);

		Task<ResultHelper<T>> NewAsync<T>(T data) where T : new();

		Task<Response> BulkInsertAsync<T>(IEnumerable<T> data) where T : new();

		Task<Response> ResetAsync();

		Task<ResultHelper<T>> UpdateAsync<T>(T data) where T : new();

		Task<Response> ExportAsync();
	}
}