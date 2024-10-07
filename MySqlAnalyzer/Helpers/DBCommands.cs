namespace MySqlAnalyzer.Helpers;
public class DBCommands
{

	#region Get Table or View Structure
	/// <summary>
	///		Retrieves the structure of all base tables in the specified database.
	///		The method queries the information schema for metadata about the tables, such as
	///		table name, type, engine, creation and update times, collation, and comments.
	/// </summary>
	/// <param name="database">The name of the database for which the table structure is retrieved.</param>
	/// <param name="tabletype">The routinetype (BASE_Table of VIEW) that should be in the return list.</param>
	/// <returns>A list of Tables or Views where each object contains metadata of a base table or view in the database.</returns>
	/// <remarks>
	///		This method specifically filters for base tables and views (ignoring other table types).
	///		Ensure that the provided database exists and that the connection string is properly configured.
	/// </remarks>
	public static List<Tables> GetStructure( string database, string tabletype )
	{
		List<Tables> tableList = [];
		var count = 0;

		#region Query
		string query = $"{DBNames.SqlSelect}" +
			$"{DBNames.InfoSchemeTableName}, " +
			$"{DBNames.InfoSchemeTableType}, " +
			$"{DBNames.InfoSchemeTableEngine}, " +
			$"{DBNames.InfoSchemeTableCreated}, " +
			$"{DBNames.InfoSchemeTableUpdated}, " +
			$"{DBNames.InfoSchemeTableCollation}, " +
			$"{DBNames.InfoSchemeTableComments}" +
			$"{DBNames.SqlFrom}" +
			$"{DBNames.InfoSchemeTables}" +
			$"{DBNames.SqlWhere}" +
			$"{DBNames.InfoSchemeTableSchema}= " +
			$"'{database}';";
		#endregion

		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using var command = new MySqlCommand( query, connection );
		using var reader = command.ExecuteReader();
		while ( reader.Read() )
		{
			if ( ( reader [ $"{DBNames.InfoSchemeTableType}" ].ToString() ?? "" ).Equals( tabletype, StringComparison.CurrentCultureIgnoreCase ) )
			{
				Tables table = new Tables
				{
					TableId = count,
					TableName = reader[$"{DBNames.InfoSchemeTableName}"].ToString(),
					TableType = reader[$"{DBNames.InfoSchemeTableType}"].ToString(),
					TableEngine = reader[$"{DBNames.InfoSchemeTableEngine}"].ToString(),
					TableCreation = reader[$"{DBNames.InfoSchemeTableCreated}"].ToString(),
					TableUpdated = reader[$"{DBNames.InfoSchemeTableUpdated}"].ToString(),
					TableCallation = reader[$"{DBNames.InfoSchemeTableCollation}"].ToString(),
					TableComments = reader[$"{DBNames.InfoSchemeTableComments}"].ToString(),
				};

				tableList.Add( table );
				count++;
			}
		}

		return tableList;
	}
	#endregion

	#region Get structure of functions or Stored Procedure
	/// <summary>
	///		Retrieves the structure of all functions and stored procedures in the specified database.
	///		The method queries the information schema for metadata about the functions and procedures, such as
	///		name, type, SQL, creation and update times and comments.
	/// </summary>
	/// <param name="database">The name of the database for which the table structure is retrieved.</param>
	/// <param name="routinetype">The routinetype (FUNCTION or PROCEDURE) that should be in the return list.</param>
	/// <returns>A list of Functions or Procedures where each object contains metadata of a function or procedure in the database.</returns>
	/// <remarks>
	///		This method specifically filters for functions and procedures (ignoring other routine types).
	///		Ensure that the provided database exists and that the connection string is properly configured.
	/// </remarks>
	public static List<Routines> GetFunctions( string database, string routinetype )
	{
		List<Routines> routineList = [];
		var count = 0;

		#region Query
		string query = $"{DBNames.SqlSelect}" +
			$"{DBNames.RoutineName}" +
			$"{DBNames.RoutineType}" +
			$"{DBNames.RoutineDataType}" +
			$"{DBNames.RoutineDefenition}" +
			$"{DBNames.RoutineDataAccess}" +
			$"{DBNames.RoutineCreated}" +
			$"{DBNames.RoutineUpdated}" +
			$"{DBNames.RoutineSQLMode}" +
			$"{DBNames.RoutineComment}" +
			$"{DBNames.SqlFrom}" +
			$"{DBNames.InfoSchemeRoutines}" +
			$"{DBNames.SqlWhere}" +
			$"{DBNames.InfoSchemeRoutineSchema}= " +
			$"'{database}';";
		#endregion

		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using var command = new MySqlCommand( query, connection );
		using var reader = command.ExecuteReader();
		while ( reader.Read() )
		{
			if ( ( reader [ $"{DBNames.RoutineType}" ].ToString() ?? "" ).Equals( routinetype, StringComparison.CurrentCultureIgnoreCase ) )
			{
				Routines routine = new Routines
				{
					RoutineId = count,
					RoutineName = reader[ $"{DBNames.RoutineName}"].ToString(),
					RoutineType = reader[ $"{DBNames.RoutineType}"].ToString(),
					RoutineDataType = reader[ $"{DBNames.RoutineDataType}"].ToString(),
					RoutineDefenition = reader[ $"{DBNames.RoutineDefenition}"].ToString(),
					RoutineDataAccess = reader[ $"{DBNames.RoutineDataAccess}"].ToString(),
					RoutineCreated = reader[ $"{DBNames.RoutineCreated}"].ToString(),
					RoutineUpdated = reader[ $"{DBNames.RoutineUpdated}"].ToString(),
					RoutineSQLMode = reader[ $"{DBNames.RoutineSQLMode}"].ToString(),
					RoutineComment = reader[ $"{DBNames.RoutineComment}"].ToString()
				};

				routineList.Add( routine );
				count++;
			}
		}
		return routineList;
	}
	#endregion

	public static void GetTableStructure( string _database, string _tableName, List<Tables> _tablesList )
	{

		_tableName = "brand";  // Specifieke tabel waarvoor je de kolommen wilt ophalen
		string query = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_KEY FROM information_schema.columns WHERE table_name = '{_tableName}' AND table_schema = '{DBNames.Database}';";

		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using ( var command = new MySqlCommand( query, connection ) )
		{
			using ( var reader = command.ExecuteReader() )
			{
				while ( reader.Read() )
				{
					string columnName = reader["COLUMN_NAME"].ToString();
					string dataType = reader["DATA_TYPE"].ToString();
					string isNullable = reader["IS_NULLABLE"].ToString();
					string columnKey = reader["COLUMN_KEY"].ToString(); // Bijvoorbeeld 'PRI' voor primaire sleutels
					Console.WriteLine( $"Kolom: {columnName}, Type: {dataType}, Nullable: {isNullable}, Key: {columnKey}" );
					//TODO: Dump in TabelProperties list
				}
			}
		}
	}

	public static void GetViews()
	{
		string viewName = "view_category";  // Naam van de view
		string query = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_KEY FROM information_schema.columns WHERE table_name = '{viewName}' AND table_schema = '{DBNames.Database}';";

		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using ( var command = new MySqlCommand( query, connection ) )
		{
			using ( var reader = command.ExecuteReader() )
			{
				while ( reader.Read() )
				{
					string columnName = reader["COLUMN_NAME"].ToString();
					string dataType = reader["DATA_TYPE"].ToString();
					string isNullable = reader["IS_NULLABLE"].ToString();
					string columnKey = reader["COLUMN_KEY"].ToString();
					Console.WriteLine( $"Kolom: {columnName}, Type: {dataType}, Nullable: {isNullable}, Key: {columnKey}" );
				}
			}
		}
	}
}
