using System;
using System.Collections.Generic;

namespace NancyActuator.Core.Health
{
    /// <summary>
    /// Carries information about the Health of a component or subsystem. Health contains a status to express the state
    /// of a component or subsystem and some additional details to carry some contextual information.
    ///
    /// Health instances can be created by using Builder's fluent API.
    /// </summary>
    /// <example>
    /// new Health.Builder().up().withDetail("version", "1.1.2").build();
    /// </example>
    public sealed class Health
    {
        /// <summary>
        /// Create a new Health instance with the specified status and details.
        /// </summary>
        /// <param name="builder">A Health builder</param>
        private Health(Builder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            Status = builder.Status;
            Details = builder.Details;
        }

        //@JsonUnwrapped
        /// <summary>
        /// Return the status of the Health.
        /// </summary>
        public Status Status { get; private set; }

        //@JsonAnyGetter
        /// <summary>
        /// Return the details of the Health.
        /// </summary>
        public Dictionary<string, object> Details { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj != null && obj is Health)
            {
                var other = (Health) obj;
                return Status.Equals(other.Status) && Details.Equals(other.Details);
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = Status.GetHashCode();
            return 13 * hashCode + Details.GetHashCode();
        }

        public override string ToString()
        {
            return Status + " " + Details;
        }

        /// <summary>
        /// Builder for creating immutable <see cref="Health"/> instances.
        /// </summary>
        public class Builder
        {
            public Status Status { get; private set; }
            public Dictionary<string, object> Details { get; private set; }

            /// <summary>
            /// Create new Builder instance.
            /// </summary>
            public Builder()
            {
                Status = Status.UNKNOWN;
                Details = new Dictionary<string, object>();
            }

            /// <summary>
            /// Create new Builder instance, setting status to given <code>status</code>.
            /// </summary>
            /// <param name="status">the <see cref="Status"/> to use</param>
            public Builder(Status status)
            {
                if (status == null)
                    throw new ArgumentNullException("status");

                Status = status;
                Details = new Dictionary<string, object>();
            }

            /// <summary>
            /// Create new Builder instance, setting status to given <code>status</code> and details
            /// to given <code>details</code>.
            /// </summary>
            /// <param name="status">the <see cref="Status"/> to use</param>
            /// <param name="details">the details <see cref="Dictionary{TKey,TValue}"/> to use</param>
            public Builder(Status status, Dictionary<string, object> details)
            {
                if (status == null)
                    throw new ArgumentNullException("status");
                if (details == null)
                    throw new ArgumentNullException("details");
                Status = status;
                Details = new Dictionary<string, object>(details);
            }

            /// <summary>
            /// Record detail for given <see cref="Exception"/>.
            /// </summary>
            /// <param name="ex">the exception</param>
            public Builder WithException(Exception ex)
            {
                if (ex == null)
                    throw new ArgumentNullException("ex");
                return WithDetail("error", ex.GetType().Name + ": " + ex.Message);
            }

            /// <summary>
            /// Record detail using given <code>key</code> and <code>value</code>.
            /// </summary>
            /// <param name="key">the detail key</param>
            /// <param name="value">the detail value</param>
            public Builder WithDetail(string key, object value)
            {
                if (key == null)
                    throw new ArgumentNullException("key");
                if (value == null)
                    throw new ArgumentNullException("value");
                Details.Add(key, value);
                return this;
            }

            /// <summary>
            /// Set status to UNKNOWN status.
            /// </summary>
            public Builder Unknown()
            {
                return WithStatus(Status.UNKNOWN);
            }

            /// <summary>
            /// Set status to UP status.
            /// </summary>
            public Builder Up()
            {
                return WithStatus(Status.UP);
            }

            /// <summary>
            /// Set status to DOWN and add details for given <see cref="Exception"/>.
            /// </summary>
            /// <param name="ex">the exception</param>
            public Builder Down(Exception ex)
            {
                return Down().WithException(ex);
            }

            /// <summary>
            /// Set status to DOWN.
            /// </summary>
            public Builder Down()
            {
                return WithStatus(Status.DOWN);
            }

            /// <summary>
            /// Set status to OUT_OF_SERVICE.
            /// </summary>
            public Builder OutOfService()
            {
                return WithStatus(Status.OUT_OF_SERVICE);
            }

            /// <summary>
            /// Set status to given <code>statusCode</code>.
            /// </summary>
            /// <param name="statusCode">the Status code</param>
            public Builder WithStatus(string statusCode)
            {
                return WithStatus(new Status(statusCode));
            }

            /// <summary>
            /// Set status to given <see cref="Status"/> instance.
            /// </summary>
            /// <param name="status">the Status</param>
            public Builder WithStatus(Status status)
            {
                Status = status;
                return this;
            }

            /// <summary>
            /// Create a new Health instance with the previously specified code and details.
            /// </summary>
            public Health Build()
            {
                return new Health(this);
            }
        }
    }
}