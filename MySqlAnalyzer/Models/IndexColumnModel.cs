namespace MySqlAnalyzer.Models
{
    public class IndexColumnModel
    {
        public string? ColumnName { get; set; } = string.Empty;
        public int? OrdinalPosition { get; set; }
        public bool IsDescending { get; set; }
        public int? SubPart { get; set; } // for prefix indexes
    }
}