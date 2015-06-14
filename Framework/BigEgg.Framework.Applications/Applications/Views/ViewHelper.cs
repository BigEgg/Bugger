using BigEgg.Framework.Applications.Applications.ViewModels;
using System;
using System.Threading;
using System.Windows.Threading;

namespace BigEgg.Framework.Applications.Applications.Views
{
    /// <summary>
    /// Provides helper methods that perform common tasks involving a view.
    /// </summary>
    public static class ViewHelper
    {
        /// <summary>
        /// Gets the ViewModel which is associated with the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The associated ViewModel, or <c>null</c> when no ViewModel was found.</returns>
        /// <exception cref="ArgumentNullException">view must not be <c>null</c>.</exception>
        public static ViewModel GetViewModel(this IView view)
        {
            Preconditions.NotNull(view, "view");

            object dataContent = view.DataContext;
            // When the DataContext is null then it might be that the ViewModel hasn't set it yet.
            // Enforce it by executing the event queue of the Dispatcher.
            if (dataContent == null && SynchronizationContext.Current is DispatcherSynchronizationContext)
            {
                DispatcherHelper.DoEvents();
                dataContent = view.DataContext;
            }
            return dataContent as ViewModel;
        }

        /// <summary>
        /// Gets the ViewModel which is associated with the specified view.
        /// </summary>
        /// <typeparam name="T">The type of the ViewModel</typeparam>
        /// <param name="view">The view.</param>
        /// <returns>The associated ViewModel, or <c>null</c> when no ViewModel was found.</returns>
        /// <exception cref="ArgumentNullException">view must not be <c>null</c>.</exception>
        public static T GetViewModel<T>(this IView view) where T : ViewModel
        {
            return GetViewModel(view) as T;
        }
    }
}
