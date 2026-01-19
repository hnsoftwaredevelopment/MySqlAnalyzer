namespace MySqlAnalyzer.Models;

public class ViewModel : DatabaseObject
{
    public string? Definition { get; set; } // SELECT statement
    public string? CheckOption { get; set; } // NONE, LOCAL, CASCADE
    public bool IsUpdatable { get; set; }
    public string? Definer { get; set; }
    public string? SecurityType { get; set; } // DEFINER, INVOKER
    public string? CharacterSet { get; set; }
    public string? Collation { get; set; }

    // Navigatie properties
    public List<ColumnModel> Columns { get; set; } = [];
    public List<string> DependentTables { get; set; } = [];
    public List<string> DependentViews { get; set; } = [];
}
