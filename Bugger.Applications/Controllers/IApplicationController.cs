namespace Bugger.Applications.Controllers
{
    /// <summary>
    /// Responsible for the application lifecycle.
    /// </summary>
    public interface IApplicationController
    {
        void Initialize();

        void Run();

        void ShutDown();
    }
}
