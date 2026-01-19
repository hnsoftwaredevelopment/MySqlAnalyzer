namespace MySqlAnalyzer.Models;

public class TableModel : DatabaseObject
{
    public string? TableType { get; set; }
    public string? Engine { get; set; } = string.Empty;
    public string? RowFormat { get; set; }
    public long TableRows { get; set; }
    public long DataLength { get; set; } // in bytes
    public long IndexLength { get; set; } // in bytes
    public string? Collation { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? CreateScript { get; set; }

    // Navigatie properties
    public List<ColumnModel> Columns { get; set; } = [];
    public List<IndexModel> Indexes { get; set; } = [];
    public List<ForeignKeyModel> ForeignKeys { get; set; } = [];
    public List<TriggerModel> Triggers { get; set; } = [];

    // Read-only properties voor gemak
    public long TotalSize => DataLength + IndexLength;
    public bool HasPrimaryKey => Indexes.Exists(i => i.IsPrimary);
    public bool HasForeignKeys => ForeignKeys.Count > 0;

    // Method om CREATE script te genereren
    public void GenerateCreateScript()
    {
        var script = new System.Text.StringBuilder();
        script.AppendLine($"CREATE TABLE `{Name}` (");

        // Columns
        for (int i = 0; i < Columns.Count; i++)
        {
            var column = Columns[i];
            script.Append($"  `{column.Name}` {column.ColumnType}");

            if (!column.IsNullable)
                script.Append(" NOT NULL");

            if (!string.IsNullOrEmpty(column.DefaultValue))
                script.Append($" DEFAULT {FormatDefaultValue(column.DefaultValue)}");

            if (!string.IsNullOrEmpty(column.Extra))
                script.Append($" {column.Extra}");

            if (i < Columns.Count - 1 || Indexes.Count > 0 || ForeignKeys.Count > 0)
                script.Append(",");

            script.AppendLine();
        }

        // Primary Key
        var primaryKey = Indexes.Find(i => i.IsPrimary);
        if (primaryKey != null && primaryKey.Columns.Count > 0)
        {
            var pkColumns = string.Join(", ", primaryKey.Columns.Select(c => $"`{c.ColumnName}`"));
            script.AppendLine($"  PRIMARY KEY ({pkColumns}),");
        }

        // Other Indexes
        foreach (var index in Indexes.Where(i => !i.IsPrimary))
        {
            if (index.Columns.Count > 0)
            {
                var indexType = index.IsUnique ? "UNIQUE" : "INDEX";
                var indexColumns = string.Join(", ", index.Columns.Select(c => $"`{c.ColumnName}`"));
                script.AppendLine($"  {indexType} `{index.Name}` ({indexColumns}),");
            }
        }

        // Foreign Keys
        foreach (var fk in ForeignKeys)
        {
            if (fk.ColumnMappings.Count > 0)
            {
                var columns = string.Join(", ", fk.ColumnMappings.Select(m => $"`{m.ColumnName}`"));
                var refColumns = string.Join(", ", fk.ColumnMappings.Select(m => $"`{m.ReferencedColumnName}`"));
                script.AppendLine($"  CONSTRAINT `{fk.Name}` FOREIGN KEY ({columns})");
                script.AppendLine($"    REFERENCES `{fk.ReferencedTable}` ({refColumns})");
                script.AppendLine($"    ON UPDATE {fk.UpdateRule}");
                script.AppendLine($"    ON DELETE {fk.DeleteRule},");
            }
        }

        // Remove trailing comma
        var scriptText = script.ToString();
        if (scriptText.EndsWith(",\n"))
        {
            script.Length -= 2;
            script.AppendLine();
        }

        script.AppendLine($") ENGINE={Engine}");

        if (!string.IsNullOrEmpty(Collation))
            script.AppendLine($"COLLATE={Collation}");

        if (!string.IsNullOrEmpty(Comment))
            script.AppendLine($"COMMENT='{Comment?.Replace("'", "''")}'");

        CreateScript = script.ToString();
    }

    private string FormatDefaultValue(string defaultValue)
    {
        if (defaultValue.ToUpper() == "CURRENT_TIMESTAMP" ||
            defaultValue.ToUpper() == "NULL")
            return defaultValue;

        return $"'{defaultValue.Replace("'", "''")}'";
    }
}