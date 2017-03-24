using System;

namespace NancyActuator.Core.Health
{
    public sealed class Status
    {
        /// <summary>
        /// <see cref="Status"/> indicating that the component or subsystem is in an unknown state.
        /// </summary>
        public static readonly Status UNKNOWN = new Status("UNKNOWN");

        /// <summary>
        /// <see cref="Status"/> indicating that the component or subsystem is functioning as expected.
        /// </summary>
        public static readonly Status UP = new Status("UP");

        /// <summary>
        /// <see cref="Status"/> indicating that the component or subsystem has suffered an unexpected failure.
        /// </summary>
        public static readonly Status DOWN = new Status("DOWN");

        /// <summary>
        /// <see cref="Status"/> indicating that the component or subsystem has been taken out of service and should not be used.
        /// </summary>
        public static readonly Status OUT_OF_SERVICE = new Status("OUT_OF_SERVICE");

        /// <summary>
        /// Create a new <see cref="Status"/> instance with the given code and an empty description.
        /// </summary>
        /// <param name="code">The <see cref="Status"/> code</param>
        public Status(string code) : this(code, "")
        {
        }

        /// <summary>
        /// Create a new <see cref="Status"/> instance with the given code and description.
        /// </summary>
        /// <param name="code">the <see cref="Status"/> code</param>
        /// <param name="description">a description of the <see cref="Status"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Status(string code, string description)
        {
            if (code == null)
                throw new ArgumentNullException("code");
            if (description == null)
                throw new ArgumentNullException("description");
            Code = code;
            Description = description;
        }

        //@JsonProperty("<see cref="Status"/>")
        /// <summary>
        /// Return the code for this <see cref="Status"/>.
        /// </summary>
        public string Code { get; private set; }

        //@JsonInclude(Include.NON_EMPTY)
        /// <summary>
        /// Return the description of this <see cref="Status"/>.
        /// </summary>
        public string Description { get; private set; }

        public override string ToString() {
            return Code;
        }

        public override int GetHashCode() {
            return Code.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == this) {
                return true;
            }
            if (obj != null && obj is Status) {
                return Code.Equals(((Status) obj).Code);
            }
            return false;
        }
    }
}