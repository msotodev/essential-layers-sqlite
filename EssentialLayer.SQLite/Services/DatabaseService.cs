using EssentialLayer.SQLite.Extensions;
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

		private readonly string? _databasePath;

		private SQLiteConnection _connection;

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

				_logger.LogInfo($"Created table at {typeof(T).Name}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

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
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public Response DeleteAll<T>()
		{
			try
			{
				int inserted = _connection.DeleteAll<T>();

				_logger.LogInfo($"Deleted all rows at {typeof(T).Name}: {inserted} rows");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public Response Drop<T>()
		{
			try
			{
				int dropped = _connection.DropTable<T>();

				_logger.LogInfo($"Droped {dropped} row at {typeof(T).Name}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public Response Execute(string script, params object[] args)
		{
			try
			{
				int affectedRows = _connection.Execute(script, args);

				_logger.LogInformation(
					$"Executed script: {script} with args: {string.Join(", ", args)}. Affected rows: {affectedRows}"
				);

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public ResultHelper<T> New<T>(T data) where T : new()
		{
			try
			{
				int inserted = _connection.Insert(data);

				_logger.LogInfo($"New row at {typeof(T).Name}: {data.Serialize()}");

				return ResultHelper<T>.Success(data);
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return ResultHelper<T>.Fail(e);
			}
		}

		public Response BulkInsert<T>(IEnumerable<T> data)
		{
			try
			{
				int inserted = _connection.InsertAll(data);

				_logger.LogInfo($"Bulked from {typeof(T).Name}: {inserted} rows");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public Response Reset()
		{
			try
			{
				_connection.Close();

				if (File.Exists(_databasePath)) File.Delete(_databasePath);

				_connection = new SQLiteConnection(_databasePath);

				_logger.LogInfo($"Database reseted at {_databasePath}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public ResultHelper<T> Update<T>(T data) where T : new()
		{
			try
			{
				int inserted = _connection.Update(data);
				string tableName = typeof(T).Name;

				_logger.LogInfo($"Updated row at {tableName}: {data.Serialize()}");

				return ResultHelper<T>.Success(data);
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

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

				_logger.LogInfo($"Database exported to {Path.Combine(_databasePath, databaseName)}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}
	}
}