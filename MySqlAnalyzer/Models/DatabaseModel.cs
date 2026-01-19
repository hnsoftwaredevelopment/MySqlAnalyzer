using System.Collections.Generic;

namespace MySqlAnalyzer.Models
{
    public class DatabaseModel
    {
        public string? Name { get; set; }
        public string? DefaultCharacterSet { get; set; }
        public string? DefaultCollation { get; set; }
        public List<TableModel> Tables { get; set; } = [];
        public List<ViewModel> Views { get; set; } = [];
        public List<StoredProcedureModel> StoredProcedures { get; set; } = [];
        public List<FunctionModel> Functions { get; set; } = [];
        public List<TriggerModel> Triggers { get; set; } = [];

        // Read-only properties voor statistieken
        public int TotalTables => Tables.Count;
        public int TotalViews => Views.Count;
        public int TotalStoredProcedures => StoredProcedures.Count;
        public int TotalFunctions => Functions.Count;
        public int TotalTriggers => Triggers.Count;
        public long TotalDataSize => Tables.Sum(t => t.DataLength);
        public long TotalIndexSize => Tables.Sum(t => t.IndexLength);
        public long TotalSize => TotalDataSize + TotalIndexSize;
    }
}