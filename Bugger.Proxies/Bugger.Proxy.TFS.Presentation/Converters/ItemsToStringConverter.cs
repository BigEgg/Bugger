using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Bugger.Proxy.TFS.Presentation.Converters
{
    public class ItemsToStringConverter : IValueConverter
    {
        private static readonly ItemsToStringConverter defaultInstance = new ItemsToStringConverter();

        public static ItemsToStringConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = string.Join("; ", value as ObservableCollection<string>);
            if (str.Length > 20)
            {
                str = str.Substring(0, 17) + "...";
            }

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
