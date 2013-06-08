using System.ComponentModel;

namespace Bugger.Applications.Services
{
    public interface IShellService : INotifyPropertyChanged
    {
        object MainView { get; }

        object UserBugsView { get; set; }

        object TeamBugsView { get; set; }
    }
}
