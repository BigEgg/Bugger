using System;
using System.Globalization;
using System.Windows.Data;

namespace Bugger.Presentation.Converters
{
    public class ByteToOpacityConverter : IValueConverter
    {
        private static readonly ByteToOpacityConverter defaultInstance = new ByteToOpacityConverter();

        public static ByteToOpacityConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToByte(value) * 1.0 / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
