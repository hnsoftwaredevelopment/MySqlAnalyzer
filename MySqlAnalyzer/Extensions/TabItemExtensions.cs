using System.Windows;
using System.Windows.Controls;

namespace MySqlAnalyzer.Extensions;

public static class TabItemExtensions
{
    public static readonly DependencyProperty TabIconProperty =
            DependencyProperty.RegisterAttached(
                "TabIcon",
                typeof(object),
                typeof(TabItemExtensions),
                new PropertyMetadata(null));

    public static readonly DependencyProperty TabTextProperty =
            DependencyProperty.RegisterAttached(
                "TabText",
                typeof(string),
                typeof(TabItemExtensions),
                new PropertyMetadata(string.Empty));

    public static object GetTabIcon ( DependencyObject obj )
    {
        return obj.GetValue ( TabIconProperty );
    }

    public static void SetTabIcon ( DependencyObject obj, object value )
    {
        obj.SetValue ( TabIconProperty, value );
    }

    public static string GetTabText ( DependencyObject obj )
    {
        return ( string ) obj.GetValue ( TabTextProperty );
    }

    public static void SetTabText ( DependencyObject obj, string value )
    {
        obj.SetValue ( TabTextProperty, value );
    }
}
