using System.Collections.Generic;
using System.Linq;

namespace NancyActuator.Core.Health
{
    /// <summary>
    /// Base <see cref="IHealthAggregator"/> implementation to allow subclasses to focus on aggregating the
    /// <see cref="Status"/> instances and not deal with contextual details etc.
    /// </summary>
    public abstract class AbstractHealthAggregator: IHealthAggregator
    {
        public Health Aggregate(Dictionary<string, Health> healths) {
            var status = AggregateStatus(healths.Select(keyValuePair => keyValuePair.Value.Status).ToList());
            var details = AggregateDetails(healths);
            return new Health.Builder(status, details).Build();
        }

        /// <summary>
        /// Return the single 'Aggregate' status that should be used from the specified candidates.
        /// </summary>
        /// <param name="candidates">the candidates</param>
        /// <returns>a single status</returns>
        protected abstract Status AggregateStatus(List<Status> candidates);

        /// <summary>
        ///  Return the map of 'Aggregate' details that should be used from the specified healths.
        /// </summary>
        /// <param name="healths">the Health instances to Aggregate</param>
        /// <returns>a map of details</returns>
        protected Dictionary<string, object> AggregateDetails(Dictionary<string, Health> healths)
        {
            return healths.Select(x => new KeyValuePair<string, object>(x.Key, x.Value))
                .ToDictionary(p=> p.Key, p=> p.Value);
        }
    }
}