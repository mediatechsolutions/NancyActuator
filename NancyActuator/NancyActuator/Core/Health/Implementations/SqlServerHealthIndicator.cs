using System;
using System.Data.SqlClient;

namespace NancyActuator.Core.Health.Implementations
{
	public class SqlServerHealthIndicator: AbstractHealthIndicator
	{
		private readonly string _connectionString;

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyActuator.Core.Health.Implementations.SqlServerHealthIndicator"/> class.
		/// </summary>
		/// <param name="connectionString">A SQLServer connection string.</param>
		public SqlServerHealthIndicator (string connectionString)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Check that a connection can be established to SqlServer and return the server version.
		/// </summary>
		/// <param name="builder">Builder of health information.</param>
		protected override void DoHealthCheck(Health.Builder builder)
		{
			SqlConnection conn = new SqlConnection(_connectionString);
			conn.Open();
			string host = conn.DataSource;
			string version = conn.ServerVersion;
			conn.Close();
			conn.Dispose();
			builder.Up().WithDetail("host", host).WithDetail("version", version);
		}
	}
}

