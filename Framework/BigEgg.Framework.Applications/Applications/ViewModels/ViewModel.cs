using BigEgg.Framework.Applications.Applications.Views;
using System;
using System.Threading;
using System.Windows.Threading;

namespace BigEgg.Framework.Applications.Applications.ViewModels
{
    /// <summary>
    /// Abstract base class for a ViewModel implementation.
    /// </summary>
    public abstract class ViewModel
    {
        private readonly IView view;


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public ViewModel(IView view)
        {
            Preconditions.NotNull(view, "view");
            this.view = view;

            // Check if the code is running within the WPF application model
            if (SynchronizationContext.Current is DispatcherSynchronizationContext)
            {
                // Set DataContext of the view has to be delayed so that the ViewModel can initialize the internal data (e.g. Commands)
                // before the view starts with DataBinding.
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate ()
                {
                    this.view.DataContext = this;
                });
            }
            else
            {
                // When the code runs outside of the WPF application model then we set the DataContext immediately.
                view.DataContext = this;
            }
        }


        /// <summary>
        /// Gets the associated view.
        /// </summary>
        public object View { get { return this.view; } }
    }
}
