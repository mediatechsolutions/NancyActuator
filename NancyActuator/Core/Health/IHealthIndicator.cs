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

        /// <summary>
        /// Return the name of the health indicator
        /// </summary>
        string GetName();
    }
}