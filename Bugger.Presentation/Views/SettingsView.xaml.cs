using BigEgg.Framework.Applications.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    [Export(typeof(ISettingsView))]
    public partial class SettingsView : UserControl, ISettingsView
    {
        private readonly Lazy<SettingsViewModel> viewModel;
    
    
        public SettingsView()
        {
            InitializeComponent();

            viewModel = new Lazy<SettingsViewModel>(() => ViewHelper.GetViewModel<SettingsViewModel>(this));
        }


        public string Title { get { return Properties.Resources.SettingsViewTitle; } }

        private SettingsViewModel ViewModel { get { return viewModel.Value; } }


        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (string item in e.RemovedItems)
            {
                ViewModel.SelectedTeamMembers.Remove(item);
            }
            foreach (string item in e.AddedItems)
            {
                ViewModel.SelectedTeamMembers.Add(item);
            }
        }
    }
}
