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
        private BugType bugType;
        private string bugIdetity;
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
            this.bugIdetity = string.Empty;
            this.bugType = BugType.Yellow;
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
            get { return bugType; }
            set { bugType = value; }
        }

        /// <summary>
        /// Gets or sets the bug identity.
        /// </summary>
        /// <value>
        /// The bug identity.
        /// </value>
        public string BugIdentity
        {
            get { return this.bugIdetity; }
            set
            {
                if (this.bugIdetity != value)
                {
                    this.bugIdetity = value;
                    RaisePropertyChanged("BugIdentity");
                }
            }
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
            set
            {
                if (this.isUpdate != value)
                {
                    this.isUpdate = value;
                    RaisePropertyChanged("IsUpdate");
                }
            }
        }
        #endregion
    }
}
