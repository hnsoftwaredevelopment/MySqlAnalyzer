using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MySqlAnalyzer.Helpers;
using MySqlAnalyzer.Services;
using MySqlAnalyzer.ViewModels;
using MySqlAnalyzer.Views;

using System;
using System.Diagnostics;
using System.Windows;

namespace MySqlAnalyzer
{
    public class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<DatabaseService>(provider =>
                    {
                        var connectionString = DBConnect.ConnectionString;
                        return new DatabaseService(connectionString);
                    });

                    services.AddTransient<MainViewModel>(provider =>
                    {
                        var databaseService = provider.GetRequiredService<DatabaseService>();
                        return new MainViewModel(databaseService);
                    });

                    services.AddSingleton<MainWindow>(provider =>
                    {
                        var viewModel = provider.GetRequiredService<MainViewModel>();
                        var window = new MainWindow();
                        window.DataContext = viewModel;
                        return window;
                    });
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                await _host.StartAsync();

                // Show main window
                var mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application startup failed: {ex.Message}",
                              "Startup Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                Shutdown();
            }

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }

    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Debug.WriteLine("[PROGRAM] Main method starting");

            var app = new App();
            app.Run();
        }
    }
}