using System;
using System.Collections.Generic;
using System.Linq;

namespace NancyActuator.Core.Health
{
    /// <summary>
    /// <see cref="IHealthIndicator"/> that returns health indications from all registered delegates.
    /// </summary>
    public class CompositeHealthIndicator : IHealthIndicator
    {
        private readonly Dictionary<string, IHealthIndicator> _indicators;

        private readonly IHealthAggregator _healthAggregator;

        /// <summary>
        /// Create a new <see cref="CompositeHealthIndicator"/>.
        /// </summary>
        /// <param name="healthAggregator">the Health aggregator</param>
        public CompositeHealthIndicator(IHealthAggregator healthAggregator) :
            this(healthAggregator, new Dictionary<string, IHealthIndicator>())
        {
        }

        /// <summary>
        /// Create a new <see cref="CompositeHealthIndicator"/> from the specified _indicators.
        /// </summary>
        /// <param name="healthAggregator">the Health aggregator</param>
        /// <param name="indicators">_indicators a map of <see cref="IHealthIndicator"/>s with the key being used as an
        /// indicator name.</param>
        public CompositeHealthIndicator(IHealthAggregator healthAggregator,
            IDictionary<string, IHealthIndicator> indicators)
        {
            if (healthAggregator == null)
                throw new ArgumentNullException("healthAggregator");
            if (indicators == null)
                throw new ArgumentNullException("indicators");
            _indicators = new Dictionary<string, IHealthIndicator>(indicators);
            _healthAggregator = healthAggregator;
        }

        public string GetName() {
            return this.GetType().Name;
        }

        public void AddHealthIndicator(string name, IHealthIndicator indicator)
        {
            _indicators.Add(name, indicator);
        }

        public Health Health()
        {
            var healths = _indicators
                .Select(x => new KeyValuePair<string, Health>(x.Key, x.Value.Health()))
                .ToDictionary(p=> p.Key, p=> p.Value);

            return _healthAggregator.Aggregate(healths);
        }
    }
}