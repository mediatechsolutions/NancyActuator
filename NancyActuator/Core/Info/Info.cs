using System;
using System.Collections.Generic;

namespace NancyActuator.Core.Info
{
    public class Info
    {
        public Dictionary<string, object> Details { get; private set; }

        private Info(Builder builder)
        {
            Details = new Dictionary<string, object>();
            foreach (var detail in builder.Content)
                Details.Add(detail.Key, detail.Value);
        }

        public object Get(string id)
        {
            return Details[id];
        }

        public T Get<T>(string id)
        {
            object value = Details[id];
            if (!(value is T))
            {
                throw new InvalidOperationException("Info entry is not of required type ["
                        + typeof(T).Name + "]: " + value);
            }
            return (T)value;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            if (obj != null && obj is Info)
            {
                Info other = (Info)obj;
                return Details.Equals(other.Details);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Details.GetHashCode();
        }

        public override string ToString()
        {
            return Details.ToString();
        }

        /// <summary>
        /// Builder for creating immutable <see cref="Info"/> instances.
        /// </summary>
        public class Builder
        {
            public Dictionary<string, object> Content { get; private set; }

            public Builder()
            {
                this.Content = new Dictionary<string, object>();
            }

            /// <summary>
            /// Record detail using given <code>key</code> and <code>value</code>.
            /// </summary>
            /// <param name="key">the detail key</param>
            /// <param name="value">the detail value</param>
            public Builder WithDetail(string key, object value)
            {
                this.Content.Add(key, value);
                return this;
            }

            /// <summary>
            /// Record several details.
            /// </summary>
            /// <param name="details">The details to record</param>
            public Builder WithDetails(Dictionary<string, object> details)
            {
                foreach (var detail in details)
                    Content.Add(detail.Key, detail.Value);
                return this;
            }

            /// <summary>
            /// Create a new <see cref="Info"/> instance based on the state of this builder.
            /// </summary>
            public Info Build()
            {
                return new Info(this);
            }
        }
    }
}
