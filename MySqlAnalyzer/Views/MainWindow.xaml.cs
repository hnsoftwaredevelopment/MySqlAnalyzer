using MySqlAnalyzer.Services;
using MySqlAnalyzer.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace MySqlAnalyzer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CopySqlToClipboard_Click ( object sender, RoutedEventArgs e )
        {
            var button = sender as Button;
            if ( button != null )
            {
                var dockPanel = button.Parent as DockPanel;
                if ( dockPanel != null )
                {
                    var textBox = dockPanel.Children.OfType<TextBox>().FirstOrDefault();
                    if ( textBox != null && !string.IsNullOrEmpty ( textBox.Text ) )
                    {
                        Clipboard.SetText ( textBox.Text );
                        MessageBox.Show ( "SQL copied to clipboard!", "Success",
                                        MessageBoxButton.OK, MessageBoxImage.Information );
                    }
                }
            }
        }
    }
}