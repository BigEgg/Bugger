using Bugger.Applications.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Bugger.Presentation.Converters
{
    public class SettingDialogStatusToIndeterminateConverter : IValueConverter
    {
        private static readonly SettingDialogStatusToIndeterminateConverter defaultInstance = new SettingDialogStatusToIndeterminateConverter();

        public static SettingDialogStatusToIndeterminateConverter Default { get { return defaultInstance; } }


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
            return type == SettingDialogStatus.InitiatingProxy || type == SettingDialogStatus.ValidatingProxySettings;
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
