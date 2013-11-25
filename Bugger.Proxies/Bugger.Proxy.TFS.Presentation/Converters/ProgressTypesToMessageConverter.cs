using System;
using System.Globalization;
using System.Windows.Data;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Presentation.Properties;

namespace Bugger.Proxy.TFS.Presentation.Converters
{
    public class ProgressTypesToMessageConverter : IValueConverter
    {
        private static readonly ProgressTypesToMessageConverter defaultInstance = new ProgressTypesToMessageConverter();

        public static ProgressTypesToMessageConverter Default { get { return defaultInstance; } }


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
            var type = (ProgressTypes)value;

            switch (type)
            {
                case ProgressTypes.OnConnectProgress:
                    return Resources.OnConnectProgress;
                case ProgressTypes.FailedOnConnect:
                    return Resources.FailedOnConnect;
                case ProgressTypes.OnGetFiledsProgress:
                    return Resources.OnGetFiledsProgress;
                case ProgressTypes.FailedOnGetFileds:
                    return Resources.FailedOnGetFileds;
                case ProgressTypes.OnAutoFillMapSettings:
                    return Resources.OnAutoFillMapSettings;
                case ProgressTypes.Success:
                    return Resources.Success;
                case ProgressTypes.SuccessWithError:
                    return Resources.SuccessWithError;
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