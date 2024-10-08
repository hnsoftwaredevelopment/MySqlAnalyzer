using System.Windows;

namespace MySqlAnalyzer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		List<TableModel> TableList = DBCommands.GetStructure(DBNames.Database, DBNames.TableTypeTable);
		List<TableModel> ViewList = DBCommands.GetStructure(DBNames.Database, DBNames.TableTypeView);
		List<RoutineModel> FunctionList = DBCommands.GetRoutines(DBNames.Database, DBNames.RoutineTypeFunction);
		List<RoutineModel> ProcedureList = DBCommands.GetRoutines(DBNames.Database, DBNames.RoutineTypeProcedure);
		List<ColumnModel> TableDefinitionList = DBCommands.GetTableStructure(DBNames.Database, TableList);
		List<ColumnModel> ViewDefinitionList = DBCommands.GetTableStructure(DBNames.Database, ViewList);
		//Console.WriteLine( TableList.Count.ToString(), ViewList.Count.ToString(), FunctionList.Count.ToString(), ProcedureList.Count.ToString() );
	}
}