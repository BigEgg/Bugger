namespace Bugger.Models
{
    /// <summary>
    /// The state to indicate whether the bug is new created or modified.
    /// </summary>
    public enum BugModifyState
    {
        /// <summary>
        /// The bug is not changed
        /// </summary>
        Normal,
        /// <summary>
        /// The bug is a new one
        /// </summary>
        New,
        /// <summary>
        /// The bug is different from last time
        /// </summary>
        Modified
    }
}
