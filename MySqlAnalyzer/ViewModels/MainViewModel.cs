using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using MySqlAnalyzer.Models;
using MySqlAnalyzer.Services;

namespace MySqlAnalyzer.ViewModels;

public class MainViewModel : BaseViewModel
    {
    private readonly DatabaseService _databaseService;
    private DatabaseModel? _database;
    private bool _isLoading;
    private string? _selectedDatabase;
    private string _statusMessage = "Ready";
    // Selection properties
    private TableModel? _selectedTable;
    private ViewModel? _selectedView;
    private StoredProcedureModel? _selectedStoredProcedure;
    private FunctionModel? _selectedFunction;
    private DatabaseObject? _selectedObject;
    private ExportService? _exportService;
    private ICommand _exportHtmlCommand;

    public ICommand ExportHtmlCommand => _exportHtmlCommand ??= new RelayCommand ( ExportToHtml );

    public TableModel? SelectedTable
        {
        get => _selectedTable;
        set
            {
            if ( SetField ( ref _selectedTable, value ) )
                {
                SelectedObject = value;
                }
            }
        }

    public ViewModel? SelectedView
        {
        get => _selectedView;
        set
            {
            if ( SetField ( ref _selectedView, value ) )
                {
                SelectedObject = value;
                }
            }
        }

    public StoredProcedureModel? SelectedStoredProcedure
        {
        get => _selectedStoredProcedure;
        set
            {
            if ( SetField ( ref _selectedStoredProcedure, value ) )
                {
                SelectedObject = value;
                OnPropertyChanged ( nameof ( IsTableSelected ) );
                OnPropertyChanged ( nameof ( IsViewSelected ) );
                OnPropertyChanged ( nameof ( IsStoredProcedureSelected ) );
                OnPropertyChanged ( nameof ( IsFunctionSelected ) );
                }
            }
        }

    public FunctionModel? SelectedFunction
        {
        get => _selectedFunction;
        set
            {
            if ( SetField ( ref _selectedFunction, value ) )
                {
                SelectedObject = value;
                OnPropertyChanged ( nameof ( IsTableSelected ) );
                OnPropertyChanged ( nameof ( IsViewSelected ) );
                OnPropertyChanged ( nameof ( IsStoredProcedureSelected ) );
                OnPropertyChanged ( nameof ( IsFunctionSelected ) );
                }
            }
        }

    public DatabaseObject? SelectedObject
        {
        get => _selectedObject;
        private set => SetField ( ref _selectedObject, value );
        }

    // Helper properties for UI
    public bool IsTableSelected => SelectedTable != null;
    public bool IsViewSelected => SelectedView != null;
    public bool IsStoredProcedureSelected => SelectedStoredProcedure != null;
    public bool IsFunctionSelected => SelectedFunction != null;
    public bool HasSelection => SelectedObject != null;

    public MainViewModel ( DatabaseService databaseService )
        {
        _databaseService = databaseService ?? throw new ArgumentNullException ( nameof ( databaseService ) );

        // Gebruik je eigen AsyncRelayCommand en RelayCommand
        LoadDatabaseCommand = new AsyncRelayCommand (
            async () => await LoadDatabaseAsync (),
            () => CanLoadDatabaseExecute () );

        RefreshDatabasesCommand = new AsyncRelayCommand (
            async () => await RefreshDatabasesAsync (),
            () => CanRefreshDatabasesExecute () );

        ExportCommand = new RelayCommand (
            ExportDatabaseInfo,
            () => CanExportExecute () );

        // Load initial database list
        _ = LoadDatabasesAsync ();
        }

    public DatabaseModel? Database
        {
        get => _database;
        set
            {
            if ( SetField ( ref _database, value ) )
                {
                // Update dependent properties
                OnPropertyChanged ( nameof ( HasDatabase ) );
                OnPropertyChanged ( nameof ( DatabaseSummary ) );
                OnPropertyChanged ( nameof ( DatabaseSize ) );
                OnPropertyChanged ( nameof ( FormattedDatabaseSize ) );

                // Also update commands
                RefreshCommandsCanExecute ();

                // Debug
                Debug.WriteLine ( $"[DEBUG] Database set to {( value != null ? value.Name : "null" )}" );
                Debug.WriteLine ( $"[DEBUG] HasDatabase should be: {HasDatabase}" );
                }
            }
        }
    public bool IsLoading
        {
        get => _isLoading;
        set
            {
            if ( SetField ( ref _isLoading, value ) )
                {
                // Notify commands that CanExecute might have changed
                RefreshCommandsCanExecute ();
                }
            }
        }

    public string? SelectedDatabase
        {
        get => _selectedDatabase;
        set
            {
            if ( SetField ( ref _selectedDatabase, value ) )
                {
                // Notify commands that CanExecute might have changed
                RefreshCommandsCanExecute ();
                }
            }
        }

    public string StatusMessage
        {
        get => _statusMessage;
        set => SetField ( ref _statusMessage, value );
        }

    public ObservableCollection<string> AvailableDatabases { get; } = new ObservableCollection<string> ();

    // Commands
    public ICommand LoadDatabaseCommand { get; }
    public ICommand RefreshDatabasesCommand { get; }
    public ICommand ExportCommand { get; }

    // CanExecute methods
    private bool CanLoadDatabaseExecute ()
        {
        return !IsLoading && !string.IsNullOrEmpty ( SelectedDatabase );
        }

    private bool CanRefreshDatabasesExecute ()
        {
        return !IsLoading;
        }

    private bool CanExportExecute ()
        {
        return Database != null && !IsLoading;
        }

    // Helper method to refresh all commands
    private void RefreshCommandsCanExecute ()
        {
        // For AsyncRelayCommand
        if ( LoadDatabaseCommand is AsyncRelayCommand loadCommand )
            loadCommand.NotifyCanExecuteChanged ();

        if ( RefreshDatabasesCommand is AsyncRelayCommand refreshCommand )
            refreshCommand.NotifyCanExecuteChanged ();

        // For RelayCommand
        if ( ExportCommand is RelayCommand exportCommand )
            exportCommand.NotifyCanExecuteChanged ();
        }
    private async Task LoadDatabaseAsync ()
        {
        if ( string.IsNullOrEmpty ( SelectedDatabase ) )
            return;

        try
            {
            IsLoading = true;
            StatusMessage = $"Loading database '{SelectedDatabase}'...";

            // Gebruik async methode
            Database = await _databaseService.AnalyzeDatabaseAsync ( SelectedDatabase );

            StatusMessage = $"Database '{SelectedDatabase}' loaded successfully";
            Debug.WriteLine ( $"Database loaded: {Database.Name}, Tables: {Database.Tables.Count}" );

            // Refresh export command state
            RefreshCommandsCanExecute ();
            }
        catch ( Exception ex )
            {
            StatusMessage = $"Error loading database: {ex.Message}";
            Debug.WriteLine ( $"Error loading database: {ex.Message}" );
            }
        finally
            {
            IsLoading = false;
            }
        }

    private async Task RefreshDatabasesAsync ()
        {
        try
            {
            IsLoading = true;
            StatusMessage = "Refreshing database list...";

            await LoadDatabasesAsync ();
            }
        catch ( Exception ex )
            {
            StatusMessage = $"Error refreshing databases: {ex.Message}";
            Debug.WriteLine ( $"Error refreshing databases: {ex.Message}" );
            }
        finally
            {
            IsLoading = false;
            }
        }

    private async Task LoadDatabasesAsync ()
        {
        try
            {
            Debug.WriteLine ( "[DEBUG] Starting LoadDatabasesAsync()" );
            StatusMessage = "Loading database list...";

            AvailableDatabases.Clear ();

            Debug.WriteLine ( "[DEBUG] Calling GetAvailableDatabasesAsync()" );
            var databases = await _databaseService.GetAvailableDatabasesAsync();
            Debug.WriteLine ( $"[DEBUG] Got {databases.Count} databases" );

            foreach ( var db in databases.OrderBy ( d => d ) )
                {
                AvailableDatabases.Add ( db );
                Debug.WriteLine ( $"[DEBUG] Added database: {db}" );
                }

            if ( AvailableDatabases.Count > 0 )
                {
                SelectedDatabase = AvailableDatabases [ 0 ];
                Debug.WriteLine ( $"[DEBUG] Selected database: {SelectedDatabase}" );
                }
            else
                {
                Debug.WriteLine ( "[DEBUG] No databases found" );
                }

            StatusMessage = $"Found {AvailableDatabases.Count} databases";
            Debug.WriteLine ( "[DEBUG] LoadDatabasesAsync completed" );
            }
        catch ( Exception ex )
            {
            Debug.WriteLine ( $"[DEBUG] Error in LoadDatabasesAsync: {ex.Message}" );
            Debug.WriteLine ( $"[DEBUG] Stack trace: {ex.StackTrace}" );

            StatusMessage = $"Error loading databases: {ex.Message}";
            }
        }

    private void ExportDatabaseInfo ()
        {
        // TODO: Implement export functionality
        StatusMessage = "Export functionality not yet implemented";
        }

    // Read-only properties for data binding
    public bool HasDatabase => Database != null;

    public string DatabaseSummary => Database != null
        ? $"{Database.TotalTables} tables, {Database.TotalViews} views, " +
          $"{Database.TotalStoredProcedures} procedures, {Database.TotalFunctions} functions"
        : "No database loaded";

    public long DatabaseSize => Database?.TotalSize ?? 0;

    public string FormattedDatabaseSize
        {
        get
            {
            if ( DatabaseSize == 0 ) return "0 B";

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = DatabaseSize;
            int order = 0;

            while ( len >= 1024 && order < sizes.Length - 1 )
                {
                order++;
                len /= 1024;
                }

            return $"{len:0.##} {sizes [ order ]}";
            }
        }

    private void ExportToHtml ()
        {
        if ( Database == null ) return;

        try
            {
            var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                Filter = "HTML Files (*.html)|*.html|All files (*.*)|*.*",
                DefaultExt = ".html",
                FileName = $"{Database.Name}_Documentation.html"
                };

            if ( saveDialog.ShowDialog () == true )
                {
                var html = _exportService.ExportToHtml(Database);
                _exportService.ExportToFile ( html, saveDialog.FileName );
                StatusMessage = $"Database exported to {saveDialog.FileName}.";
                }
            }
        catch ( Exception ex )
            {
            StatusMessage = $"Error exporting database: {ex.Message}";
            }
        }
    }