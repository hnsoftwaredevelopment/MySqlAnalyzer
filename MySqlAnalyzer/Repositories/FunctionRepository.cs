using MySqlAnalyzer.Models;

using System;
using System.Collections.Generic;
using System.Data;

namespace MySqlAnalyzer.Repositories
{
    public class FunctionRepository : BaseRepository
    {
        public FunctionRepository(string connectionString) : base(connectionString) { }

        public List<FunctionModel> GetFunctions(string databaseName)
        {
            var functions = new List<FunctionModel>();

            string query = $@"
                SELECT 
                    ROUTINE_NAME,
                    ROUTINE_TYPE,
                    DEFINER,
                    SQL_DATA_ACCESS,
                    IS_DETERMINISTIC,
                    SECURITY_TYPE,
                    CHARACTER_SET_CLIENT,
                    COLLATION_CONNECTION,
                    CREATED,
                    LAST_ALTERED,
                    ROUTINE_DEFINITION,
                    ROUTINE_COMMENT,
                    DTD_IDENTIFIER
                FROM information_schema.ROUTINES 
                WHERE ROUTINE_SCHEMA = '{databaseName}' 
                AND ROUTINE_TYPE = 'FUNCTION'
                ORDER BY ROUTINE_NAME";

            var dataTable = ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                var function = new FunctionModel
                {
                    Name = GetStringSafe(row, "ROUTINE_NAME"),
                    Schema = databaseName,
                    RoutineType = GetStringSafe(row, "ROUTINE_TYPE"),
                    Definer = GetStringSafe(row, "DEFINER"),
                    SqlDataAccess = GetStringSafe(row, "SQL_DATA_ACCESS"),
                    IsDeterministic = GetBoolSafe(row, "IS_DETERMINISTIC"),
                    SecurityType = GetStringSafe(row, "SECURITY_TYPE"),
                    CharacterSetClient = GetStringSafe(row, "CHARACTER_SET_CLIENT"),
                    CollationConnection = GetStringSafe(row, "COLLATION_CONNECTION"),
                    Created = GetDateTimeSafe(row, "CREATED") ?? DateTime.MinValue,
                    Updated = GetDateTimeSafe(row, "LAST_ALTERED") ?? DateTime.MinValue,
                    Body = GetStringSafe(row, "ROUTINE_DEFINITION"),
                    Comment = GetStringSafe(row, "ROUTINE_COMMENT"),
                    ReturnsDataType = GetStringSafe(row, "DTD_IDENTIFIER")
                };

                functions.Add(function);
            }

            return functions;
        }

        public List<ParameterModel> GetFunctionParameters(string databaseName, string functionName)
        {
            var parameters = new List<ParameterModel>();

            string query = $@"
                SELECT 
                    PARAMETER_NAME,
                    ORDINAL_POSITION,
                    PARAMETER_MODE,
                    DATA_TYPE,
                    CHARACTER_MAXIMUM_LENGTH,
                    NUMERIC_PRECISION,
                    NUMERIC_SCALE,
                    DATETIME_PRECISION,
                    CHARACTER_SET_NAME,
                    COLLATION_NAME,
                    DTD_IDENTIFIER
                FROM information_schema.PARAMETERS 
                WHERE SPECIFIC_SCHEMA = '{databaseName}' 
                AND SPECIFIC_NAME = '{functionName}'
                ORDER BY ORDINAL_POSITION";

            var dataTable = ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                var parameter = new ParameterModel
                {
                    Name = GetStringSafe(row, "PARAMETER_NAME"),
                    RoutineName = functionName,
                    RoutineType = "FUNCTION",
                    OrdinalPosition = GetIntSafe(row, "ORDINAL_POSITION"),
                    ParameterMode = GetParameterMode(GetStringSafe(row, "PARAMETER_MODE")),
                    DataType = GetStringSafe(row, "DATA_TYPE"),
                    CharacterMaximumLength = GetLongSafe(row, "CHARACTER_MAXIMUM_LENGTH"),
                    NumericPrecision = GetIntSafe(row, "NUMERIC_PRECISION"),
                    NumericScale = GetIntSafe(row, "NUMERIC_SCALE"),
                    DateTimePrecision = GetIntSafe(row, "DATETIME_PRECISION"),
                    CharacterSet = GetStringSafe(row, "CHARACTER_SET_NAME"),
                    Collation = GetStringSafe(row, "COLLATION_NAME"),
                    DtdIdentifier = GetStringSafe(row, "DTD_IDENTIFIER")
                };

                parameters.Add(parameter);
            }

            return parameters;
        }
    }
}