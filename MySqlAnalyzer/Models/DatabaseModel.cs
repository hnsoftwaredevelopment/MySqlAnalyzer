using System.Collections.Generic;

namespace MySqlAnalyzer.Models
{
    public class DatabaseModel
    {
        public string? Name { get; set; }
        public string? DefaultCharacterSet { get; set; }
        public string? DefaultCollation { get; set; }
        public List<TableModel> Tables { get; set; } = new List<TableModel>();
        public List<ViewModel> Views { get; set; } = new List<ViewModel>();
        public List<StoredProcedureModel> StoredProcedures { get; set; } = new List<StoredProcedureModel>();
        public List<FunctionModel> Functions { get; set; } = new List<FunctionModel>();
        public List<TriggerModel> Triggers { get; set; } = new List<TriggerModel>();

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