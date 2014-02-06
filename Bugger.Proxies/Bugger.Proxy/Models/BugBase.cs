using BigEgg.Framework.Foundation;
using Bugger.Domain.Models;

namespace Bugger.Proxy.ViewModels
{
    /// <summary>
    /// Abstract base class for a Bug Model implementation.
    /// </summary>
    public abstract class BugBase : Model, IBug
    {
        #region Fields
        private BugType bugType;
        private bool isUpdate;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BugBase"/> class.
        /// </summary>
        protected BugBase()
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
            set
            {
                if (this.bugType != value)
                {
                    this.bugType = value;
                    RaisePropertyChanged("Type");
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
        /// Checks is the bug had been the updated.
        /// If true, set the IsUpdate property to <c>true</c>.
        /// </summary>
        /// <param name="oldModel">The old bug.</param>
        public abstract void CheckIsUpdate(IBug oldModel);
        #endregion
        #endregion
    }
}
