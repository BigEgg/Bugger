﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BigEgg.Framework.Applications.Presentation.Converters
{
    /// <summary>
    /// Value converter that converts a boolean value to and from Visibility enumeration values.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        private static BoolToVisibilityConverter defaultInstance = new BoolToVisibilityConverter();

        /// <summary>
        /// Gets the default instance of this converter.
        /// </summary>
        public static BoolToVisibilityConverter Default { get { return defaultInstance; } }

        /// <summary>
        /// Converts a boolean value into a Visibility enumeration value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The type of the binding target property. This parameter will be ignored.</param>
        /// <param name="parameter">Use the string 'Invert' to get an inverted result (Visible and Collapsed are exchanged). 
        /// Do not specify this parameter if the default behavior is desired.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Visible when the boolean value was true; otherwise Collapsed.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (bool?)value;
            bool invert = IsInvertParameterSet(parameter);

            return invert 
                ? flag == true ? Visibility.Collapsed : Visibility.Visible
                : flag == true ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a Visibility enumeration value into a boolean value.
        /// </summary>
        /// <param name="value">The Visibility enumeration value.</param>
        /// <param name="targetType">The type of the binding target property. This parameter will be ignored.</param>
        /// <param name="parameter">Use the string 'Invert' to get an inverted result (true and false are exchanged). 
        /// Do not specify this parameter if the default behavior is desired.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>true when the Visibility enumeration value was Visible; otherwise false.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility)value;
            var invert = IsInvertParameterSet(parameter);

            return invert
                ? visibility != Visibility.Visible
                : visibility == Visibility.Visible;
        }

        private bool IsInvertParameterSet(object parameter)
        {
            var invertParameter = parameter as string;
            return !string.IsNullOrWhiteSpace(invertParameter) && string.Equals(invertParameter, "invert", StringComparison.OrdinalIgnoreCase)
                ? true
                : false;
        }
    }
}
