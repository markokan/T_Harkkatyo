using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Samuxi.WPF.Harjoitus.Properties;

namespace Samuxi.WPF.Harjoitus.Converters
{
    public class LocalizationConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return GetResxNameByValue((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("No backward support");
        }

        private string GetResxNameByValue(string value)
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("Samuxi.WPF.Harjoitus", GetType().Assembly);


            var entry =
                rm.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() == value);

            var key = entry.Key.ToString();
            return key;

        }
    }
}
