using Microsoft.VisualStudio.Imaging;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace GitBranchDiff.Converters
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var extension = (string)value;
            var result = KnownMonikers.CSFile;
            if (IconMapping.Mapping.ContainsKey(extension))
            {
                var map = IconMapping.Mapping[extension];
                PropertyInfo propertyInfo;
                propertyInfo = typeof(KnownMonikers).GetProperty(map, BindingFlags.Public | BindingFlags.Static);
                if (propertyInfo != null)
                    return propertyInfo.GetValue(null, null);
                else
                    System.Diagnostics.Trace.Write("No property");
            }
            else
            {
                System.Diagnostics.Trace.Write("Couldn't parse");
            }
                
            return result;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
