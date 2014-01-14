namespace Bugger.Domain.Models
{
    /// <summary>
    /// The interface of the base bug model.
    /// </summary>
    public interface IBug
    {
        #region Properties
        /// <summary>
        /// Gets the type of this bug.
        /// </summary>
        /// <value>
        /// The type of this bug.
        /// </value>
        BugType Type { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Checks is the new bug model had been the updated.
        /// If true, set the IsUpdate property to <c>true</c>.
        /// </summary>
        /// <param name="oldModel">The old bug model.</param>
        void CheckUpdate(IBug oldModel);
        #endregion

    }
}
