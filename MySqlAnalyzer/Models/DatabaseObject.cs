using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MySqlAnalyzer.Models;

public class DatabaseObject
{
    public string? Name { get; set; }
    public string? Schema { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string? Comment { get; set; }
}

public class Database
{
    public string? Name { get; set; }
    public string? DefaultCharacterSet { get; set; }
    public string? DefaultCollation { get; set; }
    public List<TableModel> Tables { get; set; } = [];
    public List<ViewModel> Views { get; set; } = [];
    public List<StoredProcedureModel> StoredProcedures { get; set; } = [];
    public List<FunctionModel> Functions { get; set; } = [];
    public List<TriggerModel> Triggers { get; set; } = [];
}
