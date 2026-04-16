using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace EssentialLayer.SQLite.Extensions
{
	public static class LoggerExtensions
	{
		public static void LogInfo(
			this ILogger logger,
			string message,
			[CallerMemberName] string memberName = ""
		)
		{
			logger.LogInformation(
				"Method: {MemberName} | Message: {Message}",
				memberName,
				message
			);
		}

		public static void LogErrorEx(
			this ILogger logger,
			Exception e,
			[CallerMemberName] string memberName = ""
		)
		{
			logger.LogError(
				e,
				"Method: {MemberName} | Message: {Message}",
				memberName,
				e.Message
			);
		}
	}
}