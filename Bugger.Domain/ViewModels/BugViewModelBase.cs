using BigEgg.Framework.Applications.ViewModels;
using Bugger.Domain.Models;
using Bugger.Domain.Views;

namespace Bugger.Domain.ViewModels
{
    /// <summary>
    /// Abstract base class for a Bug ViewModel implementation.
    /// </summary>
    /// <typeparam name="TView">The type of the view. Do provide an interface as type and not the concrete type itself.</typeparam>
    public abstract class BugViewModelBase<TView> : ViewModel<TView>
        where TView : IBugView
    {
        #region Fields
        private BugType type;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BugViewModelBase{TView}"/> class.
        /// And attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        protected BugViewModelBase(TView view)
            : base(view, true)
        {

        }


        #region Properties
        /// <summary>
        /// Gets or sets the type of this bug..
        /// </summary>
        /// <value>
        /// The type of this bug.
        /// </value>
        public BugType Type
        {
            get { return type; }
            set { type = value; }
        }
        #endregion
    }
}
