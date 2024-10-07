namespace MySqlAnalyzer.Helpers;
public class DBNames
{
	#region Sql commands
	public static readonly string SqlAnd = " AND ";
	public static readonly string SqlAsc = " ASC ";
	public static readonly string SqlAs = " AS ";
	public static readonly string SqlBetween = " BETWEEN ";
	public static readonly string SqlCast = "CAST( ";
	public static readonly string SqlConcat = "CONCAT( ";
	public static readonly string SqlCount = " COUNT( ";
	public static readonly string SqlCountAll = " COUNT(*) ";
	public static readonly string SqlCountUnique = " COUNT(DISTINCT ";
	public static readonly string SqlDelete = "DELETE ";
	public static readonly string SqlDeleteFrom = "DELETE FROM ";
	public static readonly string SqlDesc = " DESC ";
	public static readonly string SqlFrom = " FROM ";
	public static readonly string SqlIn = "IN ";
	public static readonly string SqlInnerJoin = "INNER JOIN ";
	public static readonly string SqlInsert = "INSERT INTO ";
	public static readonly string SqlIsNull = " IS NULL ";
	public static readonly string SqlLimit = " LIMIT ";
	public static readonly string SqlLimit1 = " LIMIT 1 ";
	public static readonly string SqlLike = " LIKE ";
	public static readonly string SqlLower = " LOWER ( ";
	public static readonly string SqlMax = "MAX( ";
	public static readonly string SqlMin = "MIN( ";
	public static readonly string SqlOn = " ON ";
	public static readonly string SqlOr = " OR ";
	public static readonly string SqlOrder = " ORDER BY ";
	public static readonly string SqlOrderBy = " ORDER BY ";
	public static readonly string SqlSelect = "SELECT ";
	public static readonly string SqlSelectAll = "SELECT *";
	public static readonly string SqlSelectDistinct = "SELECT DISTINCT ";
	public static readonly string SqlSet = " SET ";
	public static readonly string SqlSum = " SUM( ";
	public static readonly string SqlSubString = " SUBSTRING( ";
	public static readonly string SqlUnsigned = " as UNSIGNED) ";
	public static readonly string SqlUnionAll = "UNION ALL";
	public static readonly string SqlUpdate = "UPDATE ";
	public static readonly string SqlValues = " VALUES ";
	public static readonly string SqlWhere = " WHERE ";
	public static readonly string SqlWithRecursive = "WITH RECURSIVE ";
	#endregion Sql commands

	#region Database
	public static readonly string Database = "modelbuilder";
	#endregion Database

	//Database Schema Names
	public static readonly string InfoSchemeTables                  = "information_schema.tables";
	public static readonly string InfoSchemeTableSchema             = "table_schema";
	public static readonly string InfoSchemeTableName               = "TABLE_NAME";
	public static readonly string InfoSchemeTableType               = "TABLE_TYPE";
	public static readonly string InfoSchemeTableCreated            = "CREATE_TIME";
	public static readonly string InfoSchemeTableUpdated            = "UPDATE_TIME";
	public static readonly string InfoSchemeTableEngine             = "ENGINE";
	public static readonly string InfoSchemeTableCollation          = "TABLE_COLLATION";
	public static readonly string InfoSchemeTableComments           = "TABLE_COMMENT";

	public static readonly string InfoSchemeColumns                 = "information_schema.columns";
	public static readonly string InfoSchemeColumnTableName         = "TABLE_NAME";
	public static readonly string InfoSchemeColumnColumnName        = "COLUMN_NAME";
	public static readonly string InfoSchemeColumnColumnType        = "COLUMN_TYPE";
	public static readonly string InfoSchemeColumnColumnKey         = "COLUMN_KEY";
	public static readonly string InfoSchemeColumnPosition          = "ORDINAL_POSITION";
	public static readonly string InfoSchemeColumnDefault           = "COLUMN_DEFAULT";
	public static readonly string InfoSchemeColumnNullable          = "IS_NULLABLE";
	public static readonly string InfoSchemeColumnDataType          = "DATA_TYPE";
	public static readonly string InfoSchemeColumnDataLength        = "CHARACTER_MAXIMUM_LENGTH";
	public static readonly string InfoSchemeColumnNumericPrecision  = "NUMERIC_PRECISION";
	public static readonly string InfoSchemeColumnNumericScale      = "NUMERIC_SCALE";
	public static readonly string InfoSchemeColumnDateTimePrecision = "DATETIME_PRECISION";
	public static readonly string InfoSchemeColumnCharacterSet      = "CHARACTER_SET_NAME";
	public static readonly string InfoSchemeColumnCollation         = "COLLATION_NAME";
	public static readonly string InfoSchemeColumnExtra             = "EXTRA";
	public static readonly string InfoSchemeColumnPrivileges        = "PRIVILEGES";
	public static readonly string InfoSchemeColumnComment           = "COLUMN_COMMENT";

	public static readonly string InfoSchemeRoutines                = "information_schema.routines";
	public static readonly string InfoSchemeRoutineSchema           = "routine_schema";
	public static readonly string RoutineName                       = "ROUTINE_NAME";
	public static readonly string RoutineType                       = "ROUTINE_TYPE";
	public static readonly string RoutineDataType                   = "DATA_TYPE";
	public static readonly string RoutineDefenition                 = "ROUTINE_DEFINITION";
	public static readonly string RoutineDataAccess                 = "SQL_DATA_ACCESS";
	public static readonly string RoutineCreated                    = "CREATED";
	public static readonly string RoutineUpdated                    = "LAST_ALTERED";
	public static readonly string RoutineSQLMode                    = "SQL_MODE";
	public static readonly string RoutineComment                    = "ROUTINE_COMMENT";

	public static readonly string TableTypeTable                    = "BASE TABLE";
	public static readonly string TableTypeView                     = "VIEW";
	public static readonly string RoutineTypeFunction               = "FUNCTION";
	public static readonly string RoutineTypeProcedure              = "PROCEDURE";
}
