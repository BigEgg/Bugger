using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace BigEgg.Framework.Applications.Applications.Commands
{
    /// <summary>
    /// Handles management and dispatching of EventHandlers in a weak way.
    /// </summary>
    internal static class WeakEventHandlerManager
    {
        #region Public Methods
        ///<summary>
        /// Invokes the handlers 
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="handlers"></param>
        public static void CallWeakReferenceHandlers(object sender, List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.
                var callees = CleanupOldHandlers(handlers);

                // Call the handlers that we snapshotted
                foreach (var handler in callees)
                {
                    CallHandler(sender, handler);
                }
            }
        }

        ///<summary>
        /// Adds a handler to the supplied list in a weak way.
        ///</summary>
        ///<param name="handlers">Existing handler list. It will be created if null.</param>
        ///<param name="handler">Handler to add.</param>
        ///<param name="defaultListSize">Default list size.</param>
        public static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize = 2)
        {
            if (handlers == null)
            {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        ///<summary>
        /// Removes an event handler from the reference list.
        ///</summary>
        ///<param name="handlers">Handler list to remove reference from.</param>
        ///<param name="handler">Handler to remove.</param>
        public static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers != null)
            {
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    WeakReference reference = handlers[i];
                    EventHandler existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == handler))
                    {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        handlers.RemoveAt(i);
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private static void CallHandler(object sender, EventHandler eventHandler)
        {
            if (eventHandler == null) { return; }

            DispatcherProxy dispatcher = DispatcherProxy.CreateDispatcher();
            if (dispatcher != null && !dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke((Action<object, EventHandler>)CallHandler, sender, eventHandler);
            }
            else
            {
                eventHandler(sender, EventArgs.Empty);
            }
        }

        private static List<EventHandler> CleanupOldHandlers(List<WeakReference> handlers)
        {
            var result = new List<EventHandler>();
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = handlers[i];
                EventHandler handler = reference.Target as EventHandler;
                if (handler == null)
                {
                    // Clean up old handlers that have been collected
                    handlers.RemoveAt(i);
                }
                else
                {
                    result.Add(handler);
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Based on the Prism framework, for "Hides the dispatcher mis-match between Silverlight and .Net, largely so code reads a bit easier".
        /// </summary>
        private class DispatcherProxy
        {
            Dispatcher innerDispatcher;

            private DispatcherProxy(Dispatcher dispatcher)
            {
                innerDispatcher = dispatcher;
            }

            public static DispatcherProxy CreateDispatcher()
            {
                DispatcherProxy proxy = null;
                if (Application.Current == null) { return null; }

                proxy = new DispatcherProxy(Application.Current.Dispatcher);
                return proxy;
            }

            public bool CheckAccess()
            {
                return innerDispatcher.CheckAccess();
            }

            public DispatcherOperation BeginInvoke(Delegate method, params Object[] args)
            {
                return innerDispatcher.BeginInvoke(method, DispatcherPriority.Normal, args);
            }
        }
    }
}
