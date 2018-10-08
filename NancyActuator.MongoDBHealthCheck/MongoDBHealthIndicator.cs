using MongoDB.Driver;

namespace NancyActuator.Core.Health.Implementations
{
	public class MongoDBHealthIndicator : AbstractHealthIndicator
	{
		private readonly string _connectionString;

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyActuator.Core.Health.Implementations.MongoDBHealthIndicator"/> class.
		/// </summary>
		/// <param name="connectionString">A MongoDB connection string.</param>
		public MongoDBHealthIndicator(string connectionString) : base(typeof(MongoDBHealthIndicator).Name)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyActuator.Core.Health.Implementations.MongoDBHealthIndicator"/> class.
		/// </summary>
		/// <param name="connectionString"> A MongoDB connection string. </param>
		/// <param name="name"> Name of the health indicator. </param>
		public MongoDBHealthIndicator(string name, string connectionString) : base(name) {
			_connectionString = connectionString;
		}

		/// <summary>
		/// Check that a connection can be established to PostgreSQL and return the server version.
		/// </summary>
		/// <param name="builder">Builder of health information.</param>
		protected override void DoHealthCheck(Health.Builder builder)
		{
            var client = new MongoClient(_connectionString);
            builder.Up().WithDetail("server", client.Settings.Server);
		}
	}
}

