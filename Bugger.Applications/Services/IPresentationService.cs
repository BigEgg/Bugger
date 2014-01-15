namespace Bugger.Applications.Services
{
    /// <summary>
    /// The interface that define all the data that related with the presentation
    /// which the service should contains.
    /// </summary>
    public interface IPresentationService
    {
        /// <summary>
        /// Gets the width of the virtual screen.
        /// </summary>
        /// <value>
        /// The width of the virtual screen.
        /// </value>
        double VirtualScreenWidth { get; }

        /// <summary>
        /// Gets the height of the virtual screen.
        /// </summary>
        /// <value>
        /// The height of the virtual screen.
        /// </value>
        double VirtualScreenHeight { get; }


        /// <summary>
        /// Initializes the cultures.
        /// </summary>
        void InitializeCultures();
    }
}
