using System;
using Npgsql;

namespace NancyActuator.Core.Health.Implementations
{
	public class PostgreSQLHealthIndicator: AbstractHealthIndicator
	{
		private readonly string _connectionString;

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyActuator.Core.Health.Implementations.PostgreSQLHealthIndicator"/> class.
		/// </summary>
		/// <param name="connectionString">A PostgreSQL connection string.</param>
		public PostgreSQLHealthIndicator (string connectionString) : base(typeof(PostgreSQLHealthIndicator).Name)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyActuator.Core.Health.Implementations.PostgreSQLHealthIndicator"/> class.
		/// </summary>
		/// <param name="connectionString"> A PostreSQL connection string. </param>
		/// <param name="name"> Name of the health indicator. </param>
		public PostgreSQLHealthIndicator(string name, string connectionString) : base(name) {
			_connectionString = connectionString;
		}

		/// <summary>
		/// Check that a connection can be established to PostgreSQL and return the server version.
		/// </summary>
		/// <param name="builder">Builder of health information.</param>
		protected override void DoHealthCheck(Health.Builder builder)
		{
			NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
			NpgsqlConnection.ClearPool(conn);
			conn.Open();
			string host = conn.Host;
			string version = conn.PostgreSqlVersion.ToString();
			int port = conn.Port;
			conn.Close();
			conn.Dispose();
			builder.Up().WithDetail("host", host).WithDetail("port", port).WithDetail("version", version);
		}
	}
}

