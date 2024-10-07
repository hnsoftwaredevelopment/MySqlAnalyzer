namespace MySqlAnalyzer.Model;
public class Columns
{
	public int? ColumnTableId { get; set; }
	public string? ColumnTableName { get; set; }
	public string? ColumnName { get; set; }
	public string? ColumnType { get; set; }
	public string? ColumnColumnKey { get; set; }
	public string? ColumnPosition { get; set; }
	public string? ColumnDefault { get; set; }
	public string? ColumnNullable { get; set; }
	public string? ColumnDataType { get; set; }
	public string? ColumnDataLength { get; set; }
	public string? ColumnNumericPrecision { get; set; }
	public string? ColumnNumericScale { get; set; }
	public string? ColumnDateTimePrecision { get; set; }
	public string? ColumnCharacterSet { get; set; }
	public string? ColumnCollation { get; set; }
	public string? ColumnExtra { get; set; }
	public string? ColumnPrivileges { get; set; }
	public string? ColumnComment { get; set; }
}
