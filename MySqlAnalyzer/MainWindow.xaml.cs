using System.Windows;

using MySqlAnalyzer.Helpers;

namespace MySqlAnalyzer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		List<Tables> TableList = DBCommands.GetStructure(DBNames.Database, DBNames.TableTypeTable);
		List<Tables> ViewList = DBCommands.GetStructure(DBNames.Database, DBNames.TableTypeView);
		List<Routines> FunctionList = DBCommands.GetFunctions(DBNames.Database, DBNames.RoutineTypeFunction);
		List<Routines> ProcedureList = DBCommands.GetFunctions(DBNames.Database, DBNames.RoutineTypeProcedure);
	}
}