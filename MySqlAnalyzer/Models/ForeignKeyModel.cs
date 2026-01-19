using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlAnalyzer.Models;

public class ForeignKeyModel
{
    public string? Name { get; set; } = string.Empty;
    public string? TableName { get; set; } = string.Empty;
    public string? Schema { get; set; } = string.Empty;
    public string? ReferencedTable { get; set; } = string.Empty;
    public string? ReferencedSchema { get; set; } = string.Empty;
    public string? UpdateRule { get; set; } = string.Empty; // CASCADE, SET NULL, NO ACTION, etc.
    public string? DeleteRule { get; set; } = string.Empty;

    // Navigatie properties
    public List<ForeignKeyColumnMappingModel> ColumnMappings { get; set; } = [];
}

public class ForeignKeyColumnMapping
{
    public string? ColumnName { get; set; } = string.Empty;
    public string? ReferencedColumnName { get; set; } = string.Empty;
}