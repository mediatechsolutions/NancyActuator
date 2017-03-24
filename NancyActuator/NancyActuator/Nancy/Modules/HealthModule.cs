using System.Collections.Generic;
using System.Linq;
using Nancy;
using NancyActuator.Core.Health;

namespace NancyActuator.Nancy.Modules
{
    public class HealthModule: NancyModule
    {
        public HealthModule(IEnumerable<IHealthIndicator> healthIndicators)
        {
            Get["/health"] = parameters =>
            {
                var compositeHealthIndicator = new CompositeHealthIndicator(new OrderedHealthAggregator(),
                    healthIndicators.ToDictionary(hi => hi.GetType().Name, hi => hi));
                return compositeHealthIndicator.Health();
            };
        }
    }
}