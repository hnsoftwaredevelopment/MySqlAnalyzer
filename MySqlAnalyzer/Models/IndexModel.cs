using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlAnalyzer.Models;

public class IndexModel
{
    public string? Name { get; set; } = string.Empty;
    public string? TableName { get; set; } = string.Empty;
    public string? Schema { get; set; } = string.Empty;
    public bool IsUnique { get; set; }
    public bool IsPrimary { get; set; }
    public string? IndexType { get; set; } = string.Empty; // BTREE, HASH, etc.
    public string? Comment { get; set; }
    public long Cardinality { get; set; }
 
    // Navigatie properties
    public List<IndexColumnModel> Columns { get; set; } = [];
}

public class IndexColumn
{
    public string? ColumnName { get; set; }
    public int OrdinalPosition { get; set; }
    public bool IsDescending { get; set; }
    public int? SubPart { get; set; } // voor prefix indexes
}
