using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

using MySqlAnalyzer.Models;

namespace MySqlAnalyzer.Converters;

public class IndexColumnsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is List<IndexColumn> columns && columns.Count > 0)
        {
            return string.Join(", ", columns.Select(c => c.ColumnName));
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}