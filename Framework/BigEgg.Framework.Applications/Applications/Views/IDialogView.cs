namespace BigEgg.Framework.Applications.Applications.Views
{
    /// <summary>
    /// Represents a dialog view
    /// </summary>
    public interface IDialogView : IView
    {
        /// <summary>
        /// Show the dialog view with an owner.
        /// </summary>
        void ShowDialog(object owner);

        /// <summary>
        /// Close the dialog view.
        /// </summary>
        void Close();
    }
}
