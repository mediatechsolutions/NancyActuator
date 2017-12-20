using System;
using System.Collections.Generic;
using System.Linq;

namespace NancyActuator.Core.Health
{
    /// <summary>
    /// Default <see cref="IHealthAggregator"/> implementation that aggregates <see cref="Health"/>
    /// instances and determines the final system state based on a simple ordered list.
    ///
    /// If a different order is required or a new <see cref="Status"/> type will be used, the order
    /// can be set by calling <see cref="SetStatusOrder(NancyActuator.Core.Health.Status[])"/>.
    /// </summary>
    public class OrderedHealthAggregator: AbstractHealthAggregator
    {
        private List<string> statusOrder;

        /// <summary>
        /// Create a new <see cref="OrderedHealthAggregator"/> instance.
        /// </summary>
        public OrderedHealthAggregator() {
            SetStatusOrder(Status.DOWN, Status.OUT_OF_SERVICE, Status.UP, Status.UNKNOWN);
        }

        /// <summary>
        /// Set the ordering of the statuses.
        /// </summary>
        /// <param name="statusOrder">an ordered list of statuses</param>
        public void SetStatusOrder(params Status[] statusOrder) {
            SetStatusOrder(statusOrder.Select(status => status.Code).ToList());
        }

        /// <summary>
        /// Set the ordering of the statuses.
        /// </summary>
        /// <param name="statusOrder">an ordered list of the status codes</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetStatusOrder(List<string> statusOrder) {
            if (statusOrder == null)
                throw new ArgumentNullException("statusOrder");
            this.statusOrder = statusOrder;
        }

        /// <inheritdoc cref="AbstractHealthAggregator.AggregateStatus"/>>
        protected override Status AggregateStatus(List<Status> candidates) {
            // Only sort those status instances that we know about
            var filteredCandidates = new List<Status>();
            foreach (var candidate in candidates) {
                if (statusOrder.Contains(candidate.Code)) {
                    filteredCandidates.Add(candidate);
                }
            }
            // If no status is given return UNKNOWN
            if (!filteredCandidates.Any()) {
                return Status.UNKNOWN;
            }
            // Sort given Status instances by configured order
            filteredCandidates.Sort(new StatusComparator(statusOrder));
            return filteredCandidates.First();
        }

        /// <summary>
        /// <see cref="IComparer{T}"/> used to order <see cref="Status"/>.
        /// </summary>
        private class StatusComparator: IComparer<Status> {

            private readonly List<string> statusOrder;

            public StatusComparator(List<string> statusOrder) {
                this.statusOrder = statusOrder;
            }

            int IComparer<Status>.Compare(Status s1, Status s2)
            {
                var i1 = statusOrder.IndexOf(s1.Code);
                var i2 = statusOrder.IndexOf(s2.Code);
                return i1 - i2;
            }
        }
    }
}