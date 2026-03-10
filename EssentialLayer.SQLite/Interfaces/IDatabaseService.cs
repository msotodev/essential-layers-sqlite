using EssentialLayers.Helpers.Result;
using System.Collections.Generic;

namespace EssentialLayer.SQLite.Interfaces
{
	public interface IDatabaseService
	{
		Response Create<T>();

		Response Delete<T>(T data);

		Response DeleteAll<T>();

		Response Drop<T>();

		ResultHelper<T> New<T>(T data) where T : new();

		Response BulkInsert<T>(IEnumerable<T> data);

		Response Reset();

		ResultHelper<T> Update<T>(T data) where T : new();

		Response Export();
	}
}