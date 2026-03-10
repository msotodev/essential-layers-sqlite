using EssentialLayer.SQLite.Interfaces;
using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EssentialLayer.SQLite.Services
{
	internal class QueryDatabaseService : IQueryDatabaseService
	{
		private readonly ILogger _logger;

		private readonly SQLiteConnection _connection;

		public QueryDatabaseService(
			ILogger<QueryDatabaseService> logger,
			string databasePath
		)
		{
			_logger = logger;

			_connection = new SQLiteConnection(databasePath);
		}

		public IReadOnlyList<T> Query<T>(string query, params object[] args) where T : class, new()
		{
			try
			{
				Log($"Executing query: {query} with arguments: {string.Join(", ", args)}");

				List<T> list = _connection.Query<T>(query, args);

				return list;
			}
			catch (Exception e)
			{
				_logger.LogError($"\n{e.Message}\n", e);

				return new List<T>();
			}
		}

		public T? QueryFirst<T>(string query, params object[] args) where T : class, new()
		{
			try
			{
				Log($"Executing query: {query} with arguments: {string.Join(", ", args)}");

				List<T> list = _connection.Query<T>(query, args);

				return list.FirstOrDefault();
			}
			catch (Exception e)
			{
				_logger.LogError($"\n{e.Message}\n", e);

				return null;
			}
		}

		private void Log(string message) => _logger.LogInformation($"\n{message}\n");
	}
}