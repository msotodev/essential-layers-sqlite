using EssentialLayer.SQLite.Interfaces;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace EssentialLayer.SQLite.Services
{
	internal class DatabaseService : IDatabaseService
	{
		private readonly ILogger _logger;

		private readonly SQLiteConnection _connection;

		private readonly string? _databasePath;

		public DatabaseService(
			ILogger<DatabaseService> logger,
			string databasePath
		)
		{
			_logger = logger;
			_databasePath = databasePath;

			_connection = new SQLiteConnection(databasePath);
		}

		public Response Create<T>()
		{
			try
			{
				_connection.CreateTable<T>();

				return Response.Success();
			}
			catch (Exception e)
			{
				return Response.Fail(e.Message);
			}
		}

		public Response Delete<T>(T data)
		{
			try
			{
				int deleted = _connection.Delete(data);

				return Response.Success();
			}
			catch (Exception e)
			{
				return Response.Fail(e.Message);
			}
		}

		public Response DeleteAll<T>()
		{
			try
			{
				string tableName = typeof(T).Name;
				int inserted = _connection.DeleteAll<T>();

				return Response.Success();
			}
			catch (Exception e)
			{
				return Response.Fail(e.Message);
			}
		}

		public Response Drop<T>()
		{
			try
			{
				int dropped = _connection.DropTable<T>();
				string tableName = typeof(T).Name;

				Log($"Droped {dropped} row at {tableName}");

				return Response.Success();
			}
			catch (Exception e)
			{
				return Response.Fail(e.Message);
			}
		}

		public ResultHelper<T> New<T>(T data) where T : new()
		{
			try
			{
				int inserted = _connection.Insert(data);
				string tableName = typeof(T).Name;

				Log($"New row at {tableName}: {data.Serialize()}");

				return ResultHelper<T>.Success(data);
			}
			catch (Exception e)
			{
				return ResultHelper<T>.Fail(e);
			}
		}

		public Response BulkInsert<T>(IEnumerable<T> data)
		{
			try
			{
				int inserted = _connection.InsertAll(data);
				string tableName = typeof(T).Name;

				Log($"Bulked from {tableName}: {inserted} rows");

				return Response.Success();
			}
			catch (Exception e)
			{
				return Response.Fail(e.Message);
			}
		}

		public Response Reset()
		{
			try
			{
				_connection.Close();
				_connection.Dispose();

				if (File.Exists(_databasePath)) File.Delete(_databasePath);

				return Response.Success();
			}
			catch (Exception e)
			{
				return Response.Fail(e.Message);
			}
		}

		public ResultHelper<T> Update<T>(T data) where T : new()
		{
			try
			{
				int inserted = _connection.Update(data);
				string tableName = typeof(T).Name;

				Log($"Updated row at {tableName}: {data.Serialize()}");

				return ResultHelper<T>.Success(data);
			}
			catch (Exception e)
			{
				return ResultHelper<T>.Fail(e.Message);
			}
		}

		public Response Export()
		{
			try
			{
				string name = Path.GetFileNameWithoutExtension(_databasePath);
				string databaseName = $"{name}_backup.db3";

				_connection.Backup(_databasePath, databaseName);

				return Response.Success();
			}
			catch (Exception e)
			{
				return Response.Fail(e.Message);
			}
		}

		private void Log(string message) => _logger.LogInformation($"\n{message}\n");
	}
}