using EssentialLayer.SQLite.Interfaces;
using EssentialLayer.SQLite.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace EssentialLayer.SQLite
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddSQLiteInstance(
			this IServiceCollection services,
			string databaseName,
			Func<IServiceProvider, string> databasePathFactory
		)
		{
			services.AddKeyedScoped<IDatabaseService>(databaseName, (provider, _) =>
				new DatabaseService(
					provider.GetRequiredService<ILogger<DatabaseService>>(),
					databasePathFactory(provider)
				)
			);

			services.AddKeyedScoped<IAsyncDatabaseService>(databaseName, (provider, _) =>
				new AsyncDatabaseService(
					provider.GetRequiredService<ILogger<AsyncDatabaseService>>(),
					databasePathFactory(provider)
				)
			);

			services.AddKeyedScoped<IQueryDatabaseService>(databaseName, (provider, _) =>
				new QueryDatabaseService(
					provider.GetRequiredService<ILogger<QueryDatabaseService>>(),
					databasePathFactory(provider)
				)
			);

			services.AddKeyedScoped<IAsyncQueryDatabase>(databaseName, (provider, _) =>
				new AsyncQueryDatabaseService(
					provider.GetRequiredService<ILogger<AsyncQueryDatabaseService>>(),
					databasePathFactory(provider)
				)
			);

			return services;
		}

		public static IServiceCollection AddRawInstance(
			this IServiceCollection services,
			string databaseName,
			Func<Stream> rawStreamFactory,
			Func<IServiceProvider, string> targetPathFactory
		)
		{
			services.AddKeyedScoped<IDatabaseService>(databaseName, (provider, _) =>
			{
				string targetPath = targetPathFactory(provider);

				if (!File.Exists(targetPath))
				{
					using Stream source = rawStreamFactory();
					using FileStream dest = File.Create(targetPath);
					source.CopyTo(dest);
				}

				return new DatabaseService(
					provider.GetRequiredService<ILogger<DatabaseService>>(),
					targetPath
				);
			});

			services.AddKeyedScoped<IAsyncDatabaseService>(databaseName, (provider, _) =>
			{
				string targetPath = targetPathFactory(provider);

				if (!File.Exists(targetPath))
				{
					using Stream source = rawStreamFactory();
					using FileStream dest = File.Create(targetPath);
					source.CopyTo(dest);
				}

				return new AsyncDatabaseService(
					provider.GetRequiredService<ILogger<AsyncDatabaseService>>(),
					targetPath
				);
			});

			services.AddKeyedScoped<IQueryDatabaseService>(databaseName, (provider, _) =>
			{
				string targetPath = targetPathFactory(provider);

				if (!File.Exists(targetPath))
				{
					using Stream source = rawStreamFactory();
					using FileStream dest = File.Create(targetPath);
					source.CopyTo(dest);
				}

				return new QueryDatabaseService(
					provider.GetRequiredService<ILogger<QueryDatabaseService>>(),
					targetPath
				);
			});

			services.AddKeyedScoped<IAsyncQueryDatabase>(databaseName, (provider, _) =>
			{
				string targetPath = targetPathFactory(provider);

				if (!File.Exists(targetPath))
				{
					using Stream source = rawStreamFactory();
					using FileStream dest = File.Create(targetPath);
					source.CopyTo(dest);
				}

				return new AsyncQueryDatabaseService(
					provider.GetRequiredService<ILogger<AsyncQueryDatabaseService>>(),
					targetPath
				);
			});

			return services;
		}
	}
}