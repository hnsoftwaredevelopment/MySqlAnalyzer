using System.Globalization;

namespace MySqlAnalyzer.Converters;
public class FilterIconConverter : IValueConverter
{
	public object? Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is bool allowFiltering )
		{
			return allowFiltering ? Application.Current.FindResource( "Filter" ) : null;
		}
		return null;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}