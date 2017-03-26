using System;
using StackExchange.Redis;

namespace NancyActuator.Core.Health.Implementations
{
	public class RedisHealthIndicator: AbstractHealthIndicator
	{
		private readonly string _connectionString;

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyActuator.Core.Health.Implementations.SqlServerHealthIndicator"/> class.
		/// </summary>
		/// <param name="connectionString">A SQLServer connection string.</param>
		public RedisHealthIndicator(string connectionString)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Check that a connection can be established to SqlServer and return the server version.
		/// </summary>
		/// <param name="builder">Builder of health information.</param>
		protected override void DoHealthCheck(Health.Builder builder)
		{
			ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(_connectionString);
			string serverStatus = conn.GetStatus();
			conn.Close();
			builder.Up().WithDetail("serverStatus", serverStatus);
		}
	}
}

