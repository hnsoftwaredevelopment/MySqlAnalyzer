using System.Text.Json.Serialization;

namespace MySqlAnalyzer.Models;

public class ColumnModel
{
    public string? Name { get; set; }
    public string? TableName { get; set; }
    public string? Schema { get; set; }
    public string? DataType { get; set; } // int, varchar, datetime, etc.
    public string? FullDataType { get; set; } // varchar(255), decimal(10,2), etc.
    public bool IsNullable { get; set; }
    public string? DefaultValue { get; set; }
    public string? CharacterSet { get; set; }
    public string? Collation { get; set; }
    public string? ColumnKey { get; set; } // PRI, UNI, MUL
    public string? Extra { get; set; } // auto_increment, on update CURRENT_TIMESTAMP, etc.
    public string? Privileges { get; set; }
    public string? Comment { get; set; }
    public int OrdinalPosition { get; set; }
    public long CharacterMaximumLength { get; set; } // For generated columns
    public int NumericPrecision { get; set; } // For numeric columns
    public int NumericScale { get; set; }
    public int DateTimePrecision { get; set; } // For datetime columns
    public int DateTimeScale { get; set; } = 0;
    public int DecimalPrecision { get; set; } // For decimal columns
    public int DecimalScale { get; set; } = 0;
    public string? ColumnType { get; set; }
    public bool IsAutoIncrement => Extra?.ToUpper ().Contains ( "AUTO_INCREMENT" ) ?? false;
    public string? GenerationExpression { get; set; } // For generated columns
    public bool IsGenerated => !string.IsNullOrEmpty ( GenerationExpression );
    public bool IsPrimaryKey => ColumnKey == "PRI";
    public bool IsUniqueKey => ColumnKey == "UNI";
    public bool IsForeignKey { get; set; }
    public string? ForeignKeyName { get; set; }
    public string? ReferencedTable { get; set; }
    public string? ReferencedColumn { get; set; }
    public string? UpdateRule { get; set; }
    public string? DeleteRule { get; set; }

    // Bereken property voor UI
    [JsonIgnore]
    public string KeyTypeDisplay
    {
        get
        {
            if ( ColumnKey?.Contains ( "PRI" ) == true )
                return "Primary Key";

            if ( IsForeignKey && !string.IsNullOrEmpty ( ReferencedTable ) )
                return $"Foreign Key → {ReferencedTable}.{ReferencedColumn}";

            if ( ColumnKey?.Contains ( "UNI" ) == true )
                return "Unique";

            if ( ColumnKey?.Contains ( "MUL" ) == true )
                return "Indexed";

            return string.Empty;
        }
    }
}