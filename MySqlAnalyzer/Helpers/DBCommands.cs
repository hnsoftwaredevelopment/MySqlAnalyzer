namespace MySqlAnalyzer.Helpers;
/// <summary>
/// The database commands the retrieve or modify data in the database.
/// </summary>
public class DBCommands
{
	#region Get Table or View Structure
	/// <summary>
	///		Retrieves the structure of all base tables in the specified _database.
	///		The method queries the information schema for metadata about the tables, such as
	///		table name, type, engine, creation and update times, collation, and comments.
	/// </summary>
	/// <param name="_database">The name of the _database for which the table structure is retrieved.</param>
	/// <param name="_tabletype">The _routinetype (BASE_Table of VIEW) that should be in the return list.</param>
	/// <returns>A list of Tables or Views where each object contains metadata of a base table or view in the _database.</returns>
	/// <remarks>
	///		This method specifically filters for base tables and views (ignoring other table types).
	///		Ensure that the provided _database exists and that the connection string is properly configured.
	/// </remarks>
	public static List<TableModel> GetStructure( string _database, string _tabletype )
	{
		List<TableModel> tableList = [];
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
			$"'{_database}';";
		#endregion

		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using var command = new MySqlCommand( query, connection );
		using var reader = command.ExecuteReader();
		while ( reader.Read() )
		{
			if ( ( reader [ $"{DBNames.InfoSchemeTableType}" ].ToString() ?? "" ).Equals( _tabletype, StringComparison.CurrentCultureIgnoreCase ) )
			{
				TableModel table = new ()
				{
					TableId = count,
					TableName = reader[$"{DBNames.InfoSchemeTableName}"].ToString(),
					TableType = reader[$"{DBNames.InfoSchemeTableType}"].ToString(),
					TableEngine = reader[$"{DBNames.InfoSchemeTableEngine}"].ToString(),
					TableCreation = reader[$"{DBNames.InfoSchemeTableCreated}"].ToString(),
					TableUpdated = reader[$"{DBNames.InfoSchemeTableUpdated}"].ToString(),
					TableCollation = reader[$"{DBNames.InfoSchemeTableCollation}"].ToString(),
					TableComments = reader[$"{DBNames.InfoSchemeTableComments}"].ToString()
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
	///		Retrieves the structure of all functions and stored procedures in the specified _database.
	///		The method queries the information schema for metadata about the functions and procedures, such as
	///		name, type, SQL, creation and update times and comments.
	/// </summary>
	/// <param name="_database">The name of the _database for which the table structure is retrieved.</param>
	/// <param name="_routinetype">The _routinetype (FUNCTION or PROCEDURE) that should be in the return list.</param>
	/// <returns>A list of Functions or Procedures where each object contains metadata of a function or procedure in the _database.</returns>
	/// <remarks>
	///		This method specifically filters for functions and procedures (ignoring other routine types).
	///		Ensure that the provided _database exists and that the connection string is properly configured.
	/// </remarks>
	public static List<RoutineModel> GetRoutines( string _database, string _routinetype )
	{
		List<RoutineModel> routineList = [];
		var count = 0;

		#region Query
		string query = $"{DBNames.SqlSelect}" +
			$"{DBNames.RoutineName}, " +
			$"{DBNames.RoutineType}, " +
			$"{DBNames.RoutineDataType}, " +
			$"{DBNames.RoutineDefenition}, " +
			$"{DBNames.RoutineDataAccess}, " +
			$"{DBNames.RoutineCreated}, " +
			$"{DBNames.RoutineUpdated}, " +
			$"{DBNames.RoutineSQLMode}, " +
			$"{DBNames.RoutineComment}" +
			$"{DBNames.SqlFrom}" +
			$"{DBNames.InfoSchemeRoutines}" +
			$"{DBNames.SqlWhere}" +
			$"{DBNames.InfoSchemeRoutineSchema}= " +
			$"'{_database}';";
		#endregion

		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using var command = new MySqlCommand( query, connection );
		using var reader = command.ExecuteReader();
		while ( reader.Read() )
		{
			if ( ( reader [ $"{DBNames.RoutineType}" ].ToString() ?? "" ).Equals( _routinetype, StringComparison.CurrentCultureIgnoreCase ) )
			{
				RoutineModel routine = new ()
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

	#region Get Table stuctures (Tables and views column definition)
	/// <summary>
	///		Retrieves the structure of of the different tables in the specified _database.
	///		The method queries the information schema for metadata about the defined content of the tables.
	/// </summary>
	/// <param name="database">The name of the _database for which the table structure is retrieved.</param>
	/// <param name="tableList"> This is a filled list with all available tables and views
	/// <returns>A detaild list table row definitions</returns>

	public static List<ColumnModel> GetTableStructure( string _database, List<TableModel> _tablesList )
	{
		List<ColumnModel> columnList = [];



		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		foreach ( var table in _tablesList )
		{
			#region Query
			string query = $"{ DBNames.SqlSelect }" +
			$"{ DBNames.InfoSchemeColumnTableName }, " +
			$"{ DBNames.InfoSchemeColumnColumnName }, " +
			$"{ DBNames.InfoSchemeColumnColumnType }, " +
			$"{ DBNames.InfoSchemeColumnColumnKey }, " +
			$"{ DBNames.InfoSchemeColumnPosition }, " +
			$"{ DBNames.InfoSchemeColumnDefault }, " +
			$"{ DBNames.InfoSchemeColumnNullable }, " +
			$"{ DBNames.InfoSchemeColumnDataType }, " +
			$"{ DBNames.InfoSchemeColumnDataLength }, " +
			$"{ DBNames.InfoSchemeColumnNumericPrecision }, " +
			$"{ DBNames.InfoSchemeColumnNumericScale }, " +
			$"{ DBNames.InfoSchemeColumnDateTimePrecision }, " +
			$"{ DBNames.InfoSchemeColumnCharacterSet }, " +
			$"{ DBNames.InfoSchemeColumnCollation }, " +
			$"{ DBNames.InfoSchemeColumnExtra }, " +
			$"{ DBNames.InfoSchemeColumnPrivileges }, " +
			$"{ DBNames.InfoSchemeColumnComment }" +
			$"{ DBNames.SqlFrom }" +
			$"{ DBNames.InfoSchemeColumns }" +
			$"{ DBNames.SqlWhere }" +
			$"{ DBNames.InfoSchemeColumnTableName } = '{table.TableName}'" +
			$"{ DBNames.SqlOrderBy }" +
			$"{ DBNames.InfoSchemeColumnPosition };";
			#endregion

			using var command = new MySqlCommand( query, connection );
			using var reader = command.ExecuteReader();
			while ( reader.Read() )
			{
				ColumnModel Column = new()
				{
					ColumnTableId = table?.TableId,
					ColumnTableName = reader[ $"{DBNames.InfoSchemeColumnTableName}"].ToString(),
					ColumnName = reader[ $"{DBNames.InfoSchemeColumnColumnName}"].ToString(),
					ColumnType = reader[ $"{DBNames.InfoSchemeColumnColumnType}"].ToString(),
					ColumnColumnKey =reader[ $"{DBNames.InfoSchemeColumnColumnKey}"].ToString(),
					ColumnPosition = reader[ $"{DBNames.InfoSchemeColumnPosition}"].ToString(),
					ColumnDefault = reader[ $"{DBNames.InfoSchemeColumnDefault}"].ToString(),
					ColumnNullable = reader[ $"{DBNames.InfoSchemeColumnNullable}"].ToString(),
					ColumnDataType = reader[ $"{DBNames.InfoSchemeColumnDataType}"].ToString(),
					ColumnDataLength = reader[ $"{DBNames.InfoSchemeColumnDataLength}"].ToString(),
					ColumnNumericPrecision = reader[ $"{DBNames.InfoSchemeColumnNumericPrecision}"].ToString(),
					ColumnNumericScale = reader[ $"{DBNames.InfoSchemeColumnNumericScale}"].ToString(),
					ColumnDateTimePrecision = reader[ $"{DBNames.InfoSchemeColumnDateTimePrecision}"].ToString(),
					ColumnCharacterSet = reader[ $"{DBNames.InfoSchemeColumnCharacterSet}"].ToString(),
					ColumnCollation = reader[ $"{DBNames.InfoSchemeColumnCollation}"].ToString(),
					ColumnExtra = reader[ $"{DBNames.InfoSchemeColumnExtra}"].ToString(),
					ColumnPrivileges = reader[ $"{DBNames.InfoSchemeColumnPrivileges}"].ToString(),
					ColumnComment = reader[ $"{DBNames.InfoSchemeColumnComment}"].ToString()
				};

				columnList.Add( Column );
			}
		}
		return columnList;
	}
	#endregion
}
