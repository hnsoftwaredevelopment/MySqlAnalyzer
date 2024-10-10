using System.Globalization;

namespace MySqlAnalyzer.Converters;
public class AllowFilteringToVisibilityConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is bool allowFiltering )
		{
			return allowFiltering ? Visibility.Visible : Visibility.Collapsed;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}
