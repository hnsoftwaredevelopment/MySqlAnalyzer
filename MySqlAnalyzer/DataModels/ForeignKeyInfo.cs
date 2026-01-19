using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlAnalyzer.DataModels;

public class ForeignKeyInfo
{
    public string? Name { get; set; }
    public string? ColumnName { get; set; }
    public string? ReferencedTable { get; set; }
    public string? ReferencedColumn { get; set; }
    public string? DeleteRule { get; set; }
    public string? UpdateRule { get; set; }
}
