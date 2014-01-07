using System.Windows;
using System.Windows.Controls;

namespace Bugger.Proxy.TFS.Presentation.TemplateSelectors
{
    public class TFSFieldComboTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ContentPresenter presenter = (ContentPresenter)container;

            if (presenter.TemplatedParent is ComboBox)
            {
                return (DataTemplate)presenter.FindResource("TFSFieldComboCollapsed");
            }
            else // Templated parent is ComboBoxItem
            {
                return (DataTemplate)presenter.FindResource("TFSFieldComboExpanded");
            }
        }
    }
}
