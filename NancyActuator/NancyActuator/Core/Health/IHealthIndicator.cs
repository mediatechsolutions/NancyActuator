namespace NancyActuator.Core.Health
{
    /// <summary>
    /// Interface used to provide an indication of application Health.
    /// </summary>
    public interface IHealthIndicator
    {
        /// <summary>
        /// Return an indication of Health.
        /// </summary>
        Health Health();
    }
}