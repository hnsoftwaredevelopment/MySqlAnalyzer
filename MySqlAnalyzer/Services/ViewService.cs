using MySqlAnalyzer.Interfaces;

namespace MySqlAnalyzer.Services
{
    public class ViewService : IViewService
    {
        private readonly IViewRepository _viewRepository;

        public ViewService ( IViewRepository viewRepository )
        {
            _viewRepository = viewRepository ?? throw new ArgumentNullException ( nameof ( viewRepository ) );
        }

        public async Task<List<ViewModel>> GetViewsAsync ( string databaseName )
        {
            if ( string.IsNullOrWhiteSpace ( databaseName ) )
            {
                throw new ArgumentException ( "Database name cannot be null or empty", nameof ( databaseName ) );
            }

            var views = await _viewRepository.GetViewsAsync(databaseName);

            // Add column information for each view
            foreach ( var view in views )
            {
                view.Columns = await _viewRepository.GetViewColumnsAsync ( databaseName, view.ViewName );
            }

            return views;
        }

        public async Task<ViewModel?> GetViewDetailsAsync ( string databaseName, string viewName )
        {
            if ( string.IsNullOrWhiteSpace ( databaseName ) )
            {
                throw new ArgumentException ( "Database name cannot be null or empty", nameof ( databaseName ) );
            }

            if ( string.IsNullOrWhiteSpace ( viewName ) )
            {
                throw new ArgumentException ( "View name cannot be null or empty", nameof ( viewName ) );
            }

            var view = await _viewRepository.GetViewByNameAsync(databaseName, viewName);

            if ( view != null )
            {
                view.Columns = await _viewRepository.GetViewColumnsAsync ( databaseName, viewName );
            }

            return view;
        }

        public async Task<string> GetFormattedViewDefinitionAsync ( string databaseName, string viewName )
        {
            var definition = await _viewRepository.GetViewDefinitionAsync(databaseName, viewName);

            // Simple SQL formatting (can be enhanced later)
            return FormatSqlDefinition ( definition );
        }

        public async Task<bool> IsViewUpdatableAsync ( string databaseName, string viewName )
        {
            var view = await _viewRepository.GetViewByNameAsync(databaseName, viewName);
            return view?.IsUpdatable ?? false;
        }

        private string FormatSqlDefinition ( string sql )
        {
            if ( string.IsNullOrWhiteSpace ( sql ) )
            {
                return string.Empty;
            }

            // Basic formatting - can be enhanced with a proper SQL formatter
            return sql.Replace ( "SELECT", "\nSELECT" )
                     .Replace ( "FROM", "\nFROM" )
                     .Replace ( "WHERE", "\nWHERE" )
                     .Replace ( "JOIN", "\nJOIN" )
                     .Replace ( "GROUP BY", "\nGROUP BY" )
                     .Replace ( "ORDER BY", "\nORDER BY" )
                     .Replace ( "UNION", "\nUNION\n" )
                     .Trim ();
        }
    }
}