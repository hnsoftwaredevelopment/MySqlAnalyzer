namespace MySqlAnalyzer.ViewModel;
public class SharedDataModel
{
	public List<TableModel> TableList { get; set; }
	public List<TableModel> ViewList { get; set; }

	public SharedDataModel()
	{
		TableList = new List<TableModel>( DBCommands.GetStructure( DBNames.Database, DBNames.TableTypeTable ) );
		ViewList = new List<TableModel>( DBCommands.GetStructure( DBNames.Database, DBNames.TableTypeView ) );
	}
}