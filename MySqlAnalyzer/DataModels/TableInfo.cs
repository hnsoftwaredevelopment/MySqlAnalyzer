using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlAnalyzer.DataModels;

public class TableInfo
{
    public string? Name { get; set; }
    public string? Type { get; set; } // TABLE, VIEW
    public int? RowCount { get; set; }
    public DateTime? CreatedDate { get; set; }
}