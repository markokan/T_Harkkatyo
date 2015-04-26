using System;
using System.Reflection;
using System.Windows.Data;
using Samuxi.WPF.Harjoitus.Model;

namespace Samuxi.WPF.Harjoitus.Converters
{
    /// @author Marko Kangas
    /// @version 26.4.2015
    ///
    /// <summary>
    /// Localization converter to enum values.
    /// </summary>
    public class LocalizationConverter : IValueConverter
    {

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
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                if (fi != null)
                {
                    var attributes =
                        (LocalizationAttribute[]) fi.GetCustomAttributes(typeof (LocalizationAttribute), false);

                    if ((attributes.Length > 0) && !String.IsNullOrEmpty(attributes[0].ResourceName))
                    {
                        return GetResxNameByValue(attributes[0].ResourceName);
                    }
                }

                return value;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("No backward support");
        }

        /// <summary>
        /// Gets the resource by name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>resource</returns>
        private string GetResxNameByValue(string value)
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("Samuxi.WPF.Harjoitus.Properties.Resources", Assembly.GetExecutingAssembly());
            
            return rm.GetString(value);
        }
    }
}
