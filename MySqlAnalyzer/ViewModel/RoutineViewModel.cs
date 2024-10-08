namespace MySqlAnalyzer.ViewModel;
public partial class RoutineViewModel : ObservableObject
{
	[ObservableProperty]
	public int? routineId;

	[ObservableProperty]
	public string? routineName;

	[ObservableProperty]
	public string? routineType;

	[ObservableProperty]
	public string? routineDataType;

	[ObservableProperty]
	public string? routineDefenition;

	[ObservableProperty]
	public string? routineDataAccess;

	[ObservableProperty]
	public string? routineCreated;

	[ObservableProperty]
	public string? routineUpdated;

	[ObservableProperty]
	public string? routineSQLMode;

	[ObservableProperty]
	public string? routineComment;

	public List<RoutineModel> FunctionList { get; set; }
	public List<RoutineModel> ProcedureList { get; set; }


	public RoutineViewModel()
	{
		FunctionList = new List<RoutineModel>( DBCommands.GetRoutines( DBNames.Database, DBNames.RoutineTypeFunction ) );
		ProcedureList = new List<RoutineModel>( DBCommands.GetRoutines( DBNames.Database, DBNames.RoutineTypeProcedure ) );
	}
}
