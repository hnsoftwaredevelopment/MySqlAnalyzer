using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlAnalyzer.Models;

public class TriggerModel : DatabaseObject
{
    public string? EventManipulation { get; set; } // INSERT, UPDATE, DELETE
    public string? EventObjectTable { get; set; }
    public string? ActionTiming { get; set; } // BEFORE, AFTER
    public string? ActionStatement { get; set; } // trigger body
    public string? Definer { get; set; }
    public string? CharacterSet { get; set; }
    public string? Collation { get; set; }
    public string? SqlMode { get; set; }

    // Read-only properties
    public string? FullEvent => $"{ActionTiming} {EventManipulation}";
}
