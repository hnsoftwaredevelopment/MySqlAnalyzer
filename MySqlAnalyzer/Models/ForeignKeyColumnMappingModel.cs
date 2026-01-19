namespace MySqlAnalyzer.Models
{
    public class ForeignKeyColumnMappingModel
    {
        public string? ColumnName { get; set; } = string.Empty;
        public string? ReferencedColumnName { get; set; } = string.Empty;
    }
}