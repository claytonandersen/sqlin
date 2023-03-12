using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Efcore.Sqlin.IntegrationTests.tests
{
	public static class StopwatchUtilities
	{
		public static async Task<TimeSpan> ExecuteTimedTaskAsync<TContextType>(Func<DbContext, Task> func, DbContext dbContext)
			where TContextType : DbContext
		{
			var sw = new Stopwatch();
			sw.Start();
			await func(dbContext);
			sw.Stop();
			return sw.Elapsed;
		}
	}
}

