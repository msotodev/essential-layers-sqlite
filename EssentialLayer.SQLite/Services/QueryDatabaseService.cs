using EssentialLayer.SQLite.Interfaces;
using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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

		public string Query(string query, params object[] args)
		{
			try
			{
				Log($"Executing query: {query} with arguments: {string.Join(", ", args)}");

				return _connection.ExecuteScalar<string>(query, args);
			}
			catch (Exception e)
			{
				Error(e.Message);

				return string.Empty;
			}
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
				Error(e.Message);

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
				Error(e.Message);

				return null;
			}
		}

		public int Version
		{
			get
			{
				try
				{
					return _connection.Query<int>("PRAGMA user_version;").FirstOrDefault();
				}
				catch (Exception e)
				{
					_logger.LogError(e.Message);

					return 0;
				}
			}
		}

		private void Error(
			string message,
			[CallerMemberName] string memberName = nameof(SQLite)
		) => _logger.LogError(
			$"{memberName} Error: \n{message}\n"
		);

		private void Log(
			string message,
			[CallerMemberName] string memberName = nameof(SQLite)
		) => _logger.LogInformation($"{memberName} Info: \n{message}\n");
	}
}