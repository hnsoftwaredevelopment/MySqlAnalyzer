using System.Collections.ObjectModel;

using MySqlAnalyzer.Interfaces;

namespace MySqlAnalyzer.ViewModels;

public class ViewDetailsViewModel : BaseViewModel
{
    private readonly IViewService _viewService;
    private ViewModel? _selectedView;
    private string? _currentDatabase;

    public ObservableCollection<ViewModel> Views { get; } = new ();
    public ObservableCollection<ViewColumn> ViewColumns { get; } = new ();

    public ViewModel? SelectedView
    {
        get => _selectedView;
        set
        {
            if ( SetField ( ref _selectedView, value ) && value != null && _currentDatabase != null )
            {
                LoadViewDetails ( _currentDatabase, value.ViewName );
            }
        }
    }

    public string? SqlDefinitionFormatted { get; private set; }

    public ViewDetailsViewModel ( IViewService viewService )
    {
        _viewService = viewService ?? throw new ArgumentNullException ( nameof ( viewService ) );
    }

    /// <summary>
    /// Loads all views for the specified database
    /// </summary>
    public async Task LoadViewsAsync ( string databaseName )
    {
        if ( string.IsNullOrWhiteSpace ( databaseName ) )
        {
            throw new ArgumentException ( "Database name cannot be null or empty", nameof ( databaseName ) );
        }

        _currentDatabase = databaseName;
        Views.Clear ();
        ViewColumns.Clear ();
        SelectedView = null;
        SqlDefinitionFormatted = null;

        try
        {
            var views = await _viewService.GetViewsAsync(databaseName);
            foreach ( var view in views )
            {
                Views.Add ( view );
            }
        }
        catch ( Exception ex )
        {
            // Handle exception (log it and show user-friendly message)
            Console.WriteLine ( $"Error loading views: {ex.Message}" );
            // In a real app, you would use a messaging system or dialog service
        }
    }

    /// <summary>
    /// Loads detailed information for a specific view
    /// </summary>
    private async void LoadViewDetails ( string databaseName, string viewName )
    {
        if ( SelectedView == null ) return;

        try
        {
            // Load detailed view information
            var viewDetails = await _viewService.GetViewDetailsAsync(databaseName, viewName);
            if ( viewDetails != null )
            {
                // Update columns
                ViewColumns.Clear ();
                foreach ( var column in viewDetails.Columns )
                {
                    ViewColumns.Add ( column );
                }

                // Load formatted SQL definition
                SqlDefinitionFormatted = await _viewService.GetFormattedViewDefinitionAsync ( databaseName, viewName );
                OnPropertyChanged ( nameof ( SqlDefinitionFormatted ) );
            }
        }
        catch ( Exception ex )
        {
            Console.WriteLine ( $"Error loading view details: {ex.Message}" );
        }
    }

    /// <summary>
    /// Refreshes the view list
    /// </summary>
    public async Task RefreshAsync ()
    {
        if ( _currentDatabase != null )
        {
            await LoadViewsAsync ( _currentDatabase );
        }
    }
}