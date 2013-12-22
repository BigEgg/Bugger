using Bugger.Domain.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bugger.Presentation.Converters
{
    public class BugTypeToImageConverter : IValueConverter
    {
        private static readonly BugTypeToImageConverter defaultInstance = new BugTypeToImageConverter();

        public static BugTypeToImageConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is BugType)) { throw new ArgumentException("value"); }

            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("/Bugger;component/resources/imageresources.xaml", UriKind.Relative);

            if ((BugType)value == BugType.Red)
                return rd["RedGiftboxImageSource"];
            else
                return rd["YellowGiftboxImageSource"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
