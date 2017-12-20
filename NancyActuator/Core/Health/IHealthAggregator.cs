using System.Collections.Generic;

namespace NancyActuator.Core.Health
{
    public interface IHealthAggregator
    {
        /// <summary>
        /// Aggregate several given <see cref="Health"/> instances into one.
        /// </summary>
        /// <param name="healths">the Health instances to Aggregate</param>
        /// <returns>the aggregated Health</returns>
        Health Aggregate(Dictionary<string, Health> healths);
    }
}