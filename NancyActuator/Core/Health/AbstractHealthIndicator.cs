using System;

namespace NancyActuator.Core.Health
{
    /// <summary>
    /// Base <see cref="IHealthIndicator"/> implementation that encapsulates creation of a <see cref="Core.Health.Health"/>
    /// instance and error handling. It will always create a DOWN status when an <see cref="Exception"/> is thrown
    /// by <see cref="DoHealthCheck"/>.
    /// </summary>
    public abstract class AbstractHealthIndicator: IHealthIndicator
    {
        public Health Health() {
            var builder = new Health.Builder();
            try {
                DoHealthCheck(builder);
            }
            catch (Exception ex) {
                builder.Down(ex);
            }
            return builder.Build();
        }

        /// <summary>
        /// Perform a Health check, populating the given builder with the check Health information.
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void DoHealthCheck(Health.Builder builder);
    }
}