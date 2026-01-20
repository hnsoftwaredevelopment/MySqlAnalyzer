using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MySqlAnalyzer.Converters;
public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Visibility visibility && visibility == Visibility.Visible;
    }
}

public class SomeClass
{
    public void SomeMethod()
    {
        var dictionary = new System.Collections.Generic.Dictionary<string, string>();
        string? someKey = null;

        // Instead of:
        // if (dictionary.ContainsKey(someKey))

        // Use:
        if (someKey != null && dictionary.ContainsKey(someKey))
        {
            // Do something
        }

        // or

        if (!string.IsNullOrEmpty(someKey) && dictionary.ContainsKey(someKey))
        {
            // Do something
        }
    }
}