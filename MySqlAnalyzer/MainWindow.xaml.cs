namespace MySqlAnalyzer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void Execute( object sender, RoutedEventArgs e )
	{
		List<TableModel> _tables = [];
		List<TableModel> _views = [];
		List<RoutineModel> _functions = [];
		List<RoutineModel> _procedures = [];

		AddSelectedItems<TableModel>( tableDataGrid.SelectedItems, _tables, item => new TableModel
		{
			TableCollation = item.TableCollation,
			TableComments = item.TableComments,
			TableCreation = item.TableCreation,
			TableEngine = item.TableEngine,
			TableId = item.TableId,
			TableName = item.TableName,
			TableType = item.TableType,
			TableUpdated = item.TableUpdated
		} );

		AddSelectedItems<TableModel>( viewDataGrid.SelectedItems, _views, item => new TableModel
		{
			TableCollation = item.TableCollation,
			TableComments = item.TableComments,
			TableCreation = item.TableCreation,
			TableEngine = item.TableEngine,
			TableId = item.TableId,
			TableName = item.TableName,
			TableType = item.TableType,
			TableUpdated = item.TableUpdated
		} );

		AddSelectedItems<RoutineModel>( functionDataGrid.SelectedItems, _functions, item => new RoutineModel
		{
			RoutineComment = item.RoutineComment,
			RoutineCreated = item.RoutineCreated,
			RoutineDataAccess = item.RoutineDataAccess,
			RoutineDataType = item.RoutineDataType,
			RoutineDefenition = item.RoutineDefenition,
			RoutineId = item.RoutineId,
			RoutineName = item.RoutineName,
			RoutineSQLMode = item.RoutineSQLMode,
			RoutineType = item.RoutineType,
			RoutineUpdated = item.RoutineUpdated
		} );

		AddSelectedItems<RoutineModel>( procedureDataGrid.SelectedItems, _procedures, item => new RoutineModel
		{
			RoutineComment = item.RoutineComment,
			RoutineCreated = item.RoutineCreated,
			RoutineDataAccess = item.RoutineDataAccess,
			RoutineDataType = item.RoutineDataType,
			RoutineDefenition = item.RoutineDefenition,
			RoutineId = item.RoutineId,
			RoutineName = item.RoutineName,
			RoutineSQLMode = item.RoutineSQLMode,
			RoutineType = item.RoutineType,
			RoutineUpdated = item.RoutineUpdated
		} );

		// Use these 4 lists as base to itterate trough all of the lists and create terieve the enformatio for every element in the list.
		// 4 Lists make it possible to create 4 main Chapters, Tables, Views, Functions and Routines
	}

	private static void AddSelectedItems<T>( IList selectedItems, IList<T> targetList, Func<T, T> createFunc )
	{
		if ( selectedItems.Count > 0 )
		{
			foreach ( T item in selectedItems )
			{
				targetList.Add( createFunc( item ) );
			}
		}
	}

	private void SelectionChanged( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		selectedTableCount.Text = tableDataGrid.SelectedItems.Count == 0 ? "none" : tableDataGrid.SelectedItems.Count.ToString();
		selectedViewCount.Text = viewDataGrid.SelectedItems.Count == 0 ? "none" : viewDataGrid.SelectedItems.Count.ToString();
		selectedFunctionCount.Text = functionDataGrid.SelectedItems.Count == 0 ? "none" : functionDataGrid.SelectedItems.Count.ToString();
		selectedProcedureCount.Text = procedureDataGrid.SelectedItems.Count == 0 ? "none" : procedureDataGrid.SelectedItems.Count.ToString();

		var _selectedItems = tableDataGrid.SelectedItems.Count + viewDataGrid.SelectedItems.Count + functionDataGrid.SelectedItems.Count + procedureDataGrid.SelectedItems.Count;
		ExecuteButton.Visibility = _selectedItems > 0 ? Visibility.Visible : Visibility.Collapsed;

	}
}