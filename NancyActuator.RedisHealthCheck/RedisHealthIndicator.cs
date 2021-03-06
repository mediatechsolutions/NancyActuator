﻿using System;
using StackExchange.Redis;

namespace NancyActuator.Core.Health.Implementations
{
	public class RedisHealthIndicator: AbstractHealthIndicator
	{
		private readonly string _connectionString;

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyActuator.Core.Health.Implementations.RedisHealthIndicator"/> class.
		/// </summary>
		/// <param name="connectionString">A Redis connection string.</param>
		public RedisHealthIndicator(string connectionString) : base(typeof(RedisHealthIndicator).Name)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyActuator.Core.Health.Implementations.RedisHealthIndicator"/> class.
		/// </summary>
		/// <param name="connectionString"> A Redis connection string. </param>
		/// <param name="name"> Name of the health indicator. </param>
		public RedisHealthIndicator(string name, string connectionString) : base(name) {
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

