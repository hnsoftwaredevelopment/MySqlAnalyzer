namespace MySqlAnalyzer.Interfaces;

public interface IViewRepository
{
    Task<List<ViewModel>> GetViewsAsync ( string databaseName );
    Task<ViewModel?> GetViewByNameAsync ( string databaseName, string viewName );
    Task<List<ViewColumn>> GetViewColumnsAsync ( string databaseName, string viewName );
    Task<string> GetViewDefinitionAsync ( string databaseName, string viewName );
    Task<bool> ViewExistsAsync ( string databaseName, string viewName );
}
