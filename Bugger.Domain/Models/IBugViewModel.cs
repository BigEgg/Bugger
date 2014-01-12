namespace Bugger.Domain.Models
{
    /// <summary>
    /// The interface of the base bug view model.
    /// </summary>
    public interface IBugViewModel
    {
        #region Properties
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
        /// Gets the associated view.
        /// </summary>
        /// <value>
        /// The associated view.
        /// </value>
        object View { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Checks is the new bug view model had been the updated.
        /// If true, set the IsUpdate property to <c>true</c>.
        /// </summary>
        /// <param name="oldModel">The old bug view model.</param>
        /// <param name="newModel">The new bug view model.</param>
        void CheckUpdate(IBugViewModel oldModel, IBugViewModel newModel);
        #endregion
    }
}
