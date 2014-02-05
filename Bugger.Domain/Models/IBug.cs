namespace Bugger.Domain.Models
{
    public interface IBug
    {
        /// <summary>
        /// Gets or sets the type of this bug.
        /// </summary>
        /// <value>
        /// The type of this bug.
        /// </value>
        BugType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bug is update.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the bug is update; otherwise, <c>false</c>.
        /// </value>
        bool IsUpdate { get; }

        /// <summary>
        /// Checks is the bug had been the updated.
        /// If true, set the IsUpdate property to <c>true</c>.
        /// </summary>
        /// <param name="oldModel">The old bug.</param>
        void CheckIsUpdate(IBug oldBug);
    }
}
