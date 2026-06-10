namespace MySqlAnalyzer.DataModels;

public class ViewInfo
{
    public string ViewName { get; set; } = string.Empty;
    public bool IsUpdatable { get; set; }
    public string Algorithm { get; set; } = string.Empty;
    public string CheckOption { get; set; } = string.Empty;
    public string SecurityType { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string CharacterSet { get; set; } = string.Empty;
    public string Collation { get; set; } = string.Empty;
    //public List<ViewColumn> Columns { get; set; } = new ();
    public DateTime? Created { get; set; }
    public DateTime? LastAltered { get; set; }
}

public class ViewColumn
{
    public string ColumnName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public bool IsNullable { get; set; }
    public string? DefaultValue { get; set; }
    public string? Extra { get; set; }
    public string? Comment { get; set; }
    public int? OrdinalPosition { get; set; }
    public string? ColumnType { get; set; }
}
