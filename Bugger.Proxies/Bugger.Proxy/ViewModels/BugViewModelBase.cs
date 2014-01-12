using BigEgg.Framework.Applications.ViewModels;
using Bugger.Domain.Models;
using Bugger.Domain.ViewModels;
using Bugger.Proxy.Views;

namespace Bugger.Proxy.ViewModels
{
    /// <summary>
    /// Abstract base class for a Bug ViewModel implementation.
    /// </summary>
    /// <typeparam name="TView">The type of the view. Do provide an interface as type and not the concrete type itself.</typeparam>
    public abstract class BugViewModelBase<TView> : ViewModel<TView>, IBugViewModel
        where TView : IBugView
    {
        #region Fields
        private BugType bugType;
        private bool isUpdate;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BugViewModelBase{TView}"/> class.
        /// And attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        protected BugViewModelBase(TView view)
            : base(view, true)
        {
            this.bugType = BugType.Yellow;
            this.isUpdate = false;
        }


        #region Properties
        /// <summary>
        /// Gets or sets the type of this bug.
        /// </summary>
        /// <value>
        /// The type of this bug.
        /// </value>
        public BugType Type
        {
            get { return this.bugType; }
            set { this.bugType = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bug is update.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the bug is update; otherwise, <c>false</c>.
        /// </value>
        public bool IsUpdate
        {
            get { return this.isUpdate; }
            protected set
            {
                if (this.isUpdate != value)
                {
                    this.isUpdate = value;
                    RaisePropertyChanged("IsUpdate");
                }
            }
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Checks is the new bug view model had been the updated.
        /// If true, set the IsUpdate property to <c>true</c>.
        /// </summary>
        /// <param name="oldModel">The old bug view model.</param>
        public abstract void CheckUpdate(IBugViewModel oldModel);
        #endregion
        #endregion
    }
}
