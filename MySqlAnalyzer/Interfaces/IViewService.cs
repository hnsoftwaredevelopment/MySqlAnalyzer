namespace MySqlAnalyzer.Interfaces;

public interface IViewService
{
    Task<List<ViewModel>> GetViewsAsync ( string databaseName );
    Task<ViewModel?> GetViewDetailsAsync ( string databaseName, string viewName );
    Task<string> GetFormattedViewDefinitionAsync ( string databaseName, string viewName );
    Task<bool> IsViewUpdatableAsync ( string databaseName, string viewName );
}
