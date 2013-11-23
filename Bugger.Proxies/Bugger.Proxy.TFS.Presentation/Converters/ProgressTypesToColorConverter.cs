using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Bugger.Proxy.TFS.Models;

namespace Bugger.Proxy.TFS.Presentation.Converters
{
    public class ProgressTypesToColorConverter : IValueConverter
    {
        private static readonly ProgressTypesToColorConverter defaultInstance = new ProgressTypesToColorConverter();

        public static ProgressTypesToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (ProgressTypes)value;
            if (type == ProgressTypes.FailedOnConnect ||
                type == ProgressTypes.FailedOnGetFileds)
            {
                return Brushes.Red;
            }
            else if (type == ProgressTypes.SuccessWithError)
            {
                return Brushes.Orange;
            }
            else
            {
                return Brushes.Green;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
