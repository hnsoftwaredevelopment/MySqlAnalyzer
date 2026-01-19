using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlAnalyzer.Models;

public class FunctionModel : DatabaseObject
{
    public string? Definer { get; set; }
    public string? SqlDataAccess { get; set; }
    public bool IsDeterministic { get; set; }
    public string? SecurityType { get; set; }
    public string? CharacterSet { get; set; }
    public string? Collation { get; set; }
    public string? Body { get; set; }
    public string? ReturnsDataType { get; set; }
    public string? RoutineType { get; set; }
    public string? CharacterSetClient { get; set; }
    public string? CollationConnection { get; set; }

    // Navigatie properties
    public List<ParameterModel> Parameters { get; set; } = [];
}
