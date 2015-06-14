using System;

namespace BigEgg.Framework.Applications.Applications.Commands
{
    /// <summary>
    /// Provides an <see cref="ICommand"/> implementation which relays the <see cref="Execute"/> and <see cref="CanExecute"/> 
    /// method to the specified delegates.
    /// </summary>
    /// <seealso cref="DelegateCommandBase"/>
    /// <seealso cref="DelegateCommand{T}"/>
    public class DelegateCommand : DelegateCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action execute) : this(execute, () => true) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action execute, Func<bool> canExecute)
            : base((o) => execute(), (o) => canExecute())
        {
            Preconditions.NotNull(execute, "execute");
            Preconditions.NotNull(canExecute, "canExecute");
        }


        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute()
        {
            return base.CanExecute(null);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="CanExecute"/> method returns <c>false.</c></exception>
        public void Execute()
        {
            base.Execute(null);
        }
    }
}
