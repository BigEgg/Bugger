using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using Bugger.Applications.Models;

namespace Bugger.Presentation.Converters
{
    public class SettingDialogStatusToColorConverter : IValueConverter
    {
        private static readonly SettingDialogStatusToColorConverter defaultInstance = new SettingDialogStatusToColorConverter();

        public static SettingDialogStatusToColorConverter Default { get { return defaultInstance; } }


        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (SettingDialogStatus)value;
            if (type == SettingDialogStatus.InitiatingProxyFailed)
            {
                return Brushes.Red;
            }
            else if (type == SettingDialogStatus.ProxyBusy ||
                     type == SettingDialogStatus.ProxyCannotConnect ||
                     type == SettingDialogStatus.ProxyUnvalid)
            {
                return Brushes.Orange;
            }
            else
            {
                return Brushes.Green;
            }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
