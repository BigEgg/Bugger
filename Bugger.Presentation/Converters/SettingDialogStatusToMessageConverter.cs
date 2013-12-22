using Bugger.Applications.Models;
using Bugger.Presentation.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Bugger.Presentation.Converters
{
    public class SettingDialogStatusToMessageConverter : IValueConverter
    {
        private static readonly SettingDialogStatusToMessageConverter defaultInstance = new SettingDialogStatusToMessageConverter();

        public static SettingDialogStatusToMessageConverter Default { get { return defaultInstance; } }


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

            switch (type)
            {
                case SettingDialogStatus.InitiatingProxy:
                    return Resources.SettingStatusInitiatingProxy;
                case SettingDialogStatus.InitiatingProxyFailed:
                    return Resources.SettingStatusInitiatingProxyFailed;
                case SettingDialogStatus.ProxyBusy:
                    return Resources.SettingStatusProxyBusy;
                case SettingDialogStatus.ProxyCannotConnect:
                    return Resources.SettingStatusProxyCannotConnect;
                case SettingDialogStatus.ProxyUnvalid:
                    return Resources.SettingStatusProxyUnvalid;
                case SettingDialogStatus.ValidatingProxySettings:
                    return Resources.SettingStatusValidatingProxySettings;
                default:
                    return string.Empty;
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
