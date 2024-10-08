namespace MySqlAnalyzer.ViewModel;
public partial class ColumnViewModel : ObservableObject
{
	[ObservableProperty]
	public int? columnTableId;

	[ObservableProperty]
	public string? columnTableName;

	[ObservableProperty]
	public string? columnName;

	[ObservableProperty]
	public string? columnType;

	[ObservableProperty]
	public string? columnColumnKey;

	[ObservableProperty]
	public string? columnPosition;

	[ObservableProperty]
	public string? columnDefault;

	[ObservableProperty]
	public string? columnNullable;

	[ObservableProperty]
	public string? columnDataType;

	[ObservableProperty]
	public string? columnDataLength;

	[ObservableProperty]
	public string? columnNumericPrecision;

	[ObservableProperty]
	public string? columnNumericScale;

	[ObservableProperty]
	public string? columnDateTimePrecision;

	[ObservableProperty]
	public string? columnCharacterSet;

	[ObservableProperty]
	public string? columnCollation;

	[ObservableProperty]
	public string? columnExtra;

	[ObservableProperty]
	public string? columnPrivileges;

	[ObservableProperty]
	public string? columnComment;

	private readonly SharedDataModel _sharedDataModel;

	public List<ColumnModel> TableDefinitionList { get; set; }
	public List<ColumnModel> ViewDefinitionList { get; set; }

	public ColumnViewModel( SharedDataModel sharedDataModel )
	{
		_sharedDataModel = sharedDataModel;
		TableDefinitionList = new List<ColumnModel>( DBCommands.GetTableStructure( DBNames.Database, _sharedDataModel.TableList ) );
		ViewDefinitionList = new List<ColumnModel>( DBCommands.GetTableStructure( DBNames.Database, _sharedDataModel.ViewList ) );
	}
}
