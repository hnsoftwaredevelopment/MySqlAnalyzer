namespace MySqlAnalyzer.Converters;
public class ListCountToVisibilityConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		if ( value is ICollection collection )
		{
			bool invert = parameter != null && bool.TryParse(parameter.ToString(), out bool result) && result;

			// If invert is true, return Collapsed when the list is NOT empty
			if ( invert )
				return collection.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

			// Default behavior: return Visible when the list has items
			return collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
		}

		return Visibility.Collapsed;
	}

	public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}
