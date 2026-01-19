using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

using MySqlAnalyzer.Models;

namespace MySqlAnalyzer.Converters;

public class ForeignKeyColumnsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is List<ForeignKeyColumnMapping> mappings && mappings.Count > 0)
        {
            var fromColumns = string.Join(", ", mappings.Select(m => m.ColumnName));
            var toColumns = string.Join(", ", mappings.Select(m => m.ReferencedColumnName));
            return $"{fromColumns} → {toColumns}";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}