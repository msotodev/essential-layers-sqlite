using EssentialLayer.SQLite.Extensions;
using EssentialLayer.SQLite.Interfaces;
using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EssentialLayer.SQLite.Services
{
	internal class AsyncQueryDatabaseService : IAsyncQueryDatabase
	{
		private readonly ILogger _logger;

		private readonly SQLiteAsyncConnection _connection;

		public AsyncQueryDatabaseService(
			ILogger<AsyncQueryDatabaseService> logger,
			string databasePath
		)
		{
			_logger = logger;

			_connection = new SQLiteAsyncConnection(databasePath);
		}

		public Task<string> QueryAsync(string query, params object[] args)
		{
			try
			{
				_logger.LogInfo($"Executing query: {query} with arguments: {string.Join(", ", args)}");

				return _connection.ExecuteScalarAsync<string>(query, args);
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Task.FromResult(string.Empty);
			}
		}

		public async Task<IReadOnlyList<T>> QueryAsync<T>(string query, params object[] args) where T : class, new()
		{
			try
			{
				_logger.LogInfo($"Executing query: {query} with arguments: {string.Join(", ", args)}");

				List<T> list = await _connection.QueryAsync<T>(query, args);

				return list;
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return new List<T>();
			}
		}

		public async Task<T?> QueryFirstsync<T>(string query, params object[] args) where T : class, new()
		{
			try
			{
				_logger.LogInfo($"Executing query: {query} with arguments: {string.Join(", ", args)}");

				List<T> list = await _connection.QueryAsync<T>(query, args);

				return list.FirstOrDefault();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return null;
			}
		}
	}
}