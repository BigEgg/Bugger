namespace Bugger.PlugIns.TrackingSystem
{
    /// <summary>
    /// The tracking system status
    /// </summary>
    public enum TrackingSystemStatus
    {
        /// <summary>
        /// Status unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// The tracking system configuration is not valid
        /// </summary>
        ConfigurationNotValid,
        /// <summary>
        /// Trying to connect to tracking system
        /// </summary>
        TryConnection,
        /// <summary>
        /// Failed to connect to tracking system
        /// </summary>
        ConnectionFailed,
        /// <summary>
        /// Connect to tracking system success
        /// </summary>
        CanConnect,
        /// <summary>
        /// Querying the bugs
        /// </summary>
        Querying
    }
}
