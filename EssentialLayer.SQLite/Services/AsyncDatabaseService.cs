using EssentialLayer.SQLite.Extensions;
using EssentialLayer.SQLite.Interfaces;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EssentialLayer.SQLite.Services
{
	public class AsyncDatabaseService : IAsyncDatabaseService
	{
		private readonly ILogger _logger;

		private readonly string _databasePath = string.Empty;

		private SQLiteAsyncConnection _connection;

		public AsyncDatabaseService(
			ILogger<AsyncDatabaseService> logger,
			string databasePath
		)
		{
			_logger = logger;
			_databasePath = databasePath;
			_connection = new SQLiteAsyncConnection(databasePath);
		}

		public async Task<Response> BulkInsertAsync<T>(IEnumerable<T> data) where T : new()
		{
			try
			{
				int inserted = await _connection.InsertAllAsync(data);

				_logger.LogInfo($"Bulked from {typeof(T).Name}: {inserted} rows");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public async Task<Response> CreateAsync<T>() where T : new()
		{
			try
			{
				await _connection.CreateTableAsync<T>();

				_logger.LogInfo($"Created table for {typeof(T).Name}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public async Task<Response> DeleteAllAsync<T>()
		{
			try
			{
				int deleted = await _connection.DeleteAllAsync<T>();

				_logger.LogInfo($"Deleted all rows at {typeof(T).Name}: {deleted} rows");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public async Task<Response> DeleteAsync<T>(T data) where T : new()
		{
			try
			{
				int deleted = await _connection.DeleteAsync(data);

				_logger.LogInfo($"Deleted row at {typeof(T).Name}: {data.Serialize()}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public async Task<Response> DropAsync<T>() where T : new()
		{
			try
			{
				int dropped = await _connection.DropTableAsync<T>();

				_logger.LogInfo($"Droped {dropped} row at {typeof(T).Name}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public async Task<Response> ExecuteAsync(string script, params object[] args)
		{
			try
			{
				int affectedRows = await _connection.ExecuteAsync(script, args);

				_logger.LogInfo(
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

		public async Task<Response> ExportAsync()
		{
			try
			{
				string name = Path.GetFileNameWithoutExtension(_databasePath);

				string databaseName = $"{name}_backup.db3";

				await _connection.BackupAsync(_databasePath, databaseName);

				_logger.LogInfo($"Exported database to {databaseName} at {Path.Combine(_databasePath, databaseName)}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public async Task<ResultHelper<T>> NewAsync<T>(T data) where T : new()
		{
			try
			{
				int inserted = await _connection.InsertAsync(data);

				_logger.LogInfo($"New row at {typeof(T).Name}: {data.Serialize()}");

				return ResultHelper<T>.Success(data);
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return ResultHelper<T>.Fail(e);
			}
		}

		public async Task<Response> ResetAsync()
		{
			try
			{
				await _connection.CloseAsync();

				if (File.Exists(_databasePath)) File.Delete(_databasePath);

				_connection = new SQLiteAsyncConnection(_databasePath);

				_logger.LogInfo($"Reseted database at {_databasePath}");

				return Response.Success();
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return Response.Fail(e.Message);
			}
		}

		public async Task<ResultHelper<T>> UpdateAsync<T>(T data) where T : new()
		{
			try
			{
				int inserted = await _connection.UpdateAsync(data);

				_logger.LogInfo($"Updated row at {typeof(T).Name}: {data.Serialize()}");

				return ResultHelper<T>.Success(data);
			}
			catch (Exception e)
			{
				_logger.LogErrorEx(e);

				return ResultHelper<T>.Fail(e.Message);
			}
		}
	}
}