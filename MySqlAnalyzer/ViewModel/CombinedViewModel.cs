namespace MySqlAnalyzer.ViewModel;
public class CombinedViewModel : ObservableObject
{
	public TableViewModel TableViewModel { get; set; }
	public ColumnViewModel ColumnViewModel { get; set; }
	public RoutineViewModel RoutineViewModel { get; set; }

	private readonly SharedDataModel _sharedDataModel;

	public CombinedViewModel()
	{
		_sharedDataModel = new();

		TableViewModel = new( _sharedDataModel );
		RoutineViewModel = new();
		ColumnViewModel = new ColumnViewModel( _sharedDataModel );
	}
}
