namespace MySqlAnalyzer.ViewModel;
public partial class TableViewModel : ObservableObject
{
	[ObservableProperty]
	public int? tableId;

	[ObservableProperty]
	public string? tableName;

	[ObservableProperty]
	public string? tableType;

	[ObservableProperty]
	public string? tableEngine;

	[ObservableProperty]
	public string? tableCreation;

	[ObservableProperty]
	public string? tableUpdated;

	[ObservableProperty]
	public string? tableCallation;

	[ObservableProperty]
	public string? tableComments;

	private readonly SharedDataModel _sharedDataModel;

	public List<TableModel> TableList => _sharedDataModel.TableList;
	public List<TableModel> ViewList => _sharedDataModel.ViewList;

	public TableViewModel( SharedDataModel sharedDataModel )
	{
		_sharedDataModel = sharedDataModel;
	}
}
