using MySqlConnection = MySqlConnector.MySqlConnection;
using MySqlCommand = MySqlConnector.MySqlCommand;

namespace MySqlAnalyzer.Helpers;
public class DBCommands
{
    public static void GetStructure()
    {
        string query = $"SELECT TABLE_NAME, TABLE_TYPE FROM information_schema.tables WHERE table_schema = '{DBNames.Database}';";

        MySqlConnection connection = new(DBConnect.ConnectionString);
        connection.Open();

        using (var command = new MySqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tableName = reader["TABLE_NAME"].ToString();
                    string tableType = reader["TABLE_TYPE"].ToString(); // 'BASE TABLE' for tables, 'VIEW' for views
                    Console.WriteLine($"Tabel/View: {tableName}, Type: {tableType}");
                    //TODO: Dump in a list for Tables, and Views
                }
            }
        }
    }

    public static void GetTableStructure(string _tableName)
    {
        _tableName = "brand";  // Specifieke tabel waarvoor je de kolommen wilt ophalen
        string query = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_KEY FROM information_schema.columns WHERE table_name = '{_tableName}' AND table_schema = '{DBNames.Database}';";

        MySqlConnection connection = new(DBConnect.ConnectionString);
        connection.Open();

        using (var command = new MySqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string columnName = reader["COLUMN_NAME"].ToString();
                    string dataType = reader["DATA_TYPE"].ToString();
                    string isNullable = reader["IS_NULLABLE"].ToString();
                    string columnKey = reader["COLUMN_KEY"].ToString(); // Bijvoorbeeld 'PRI' voor primaire sleutels
                    Console.WriteLine($"Kolom: {columnName}, Type: {dataType}, Nullable: {isNullable}, Key: {columnKey}");
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

        using (var command = new MySqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string columnName = reader["COLUMN_NAME"].ToString();
                    string dataType = reader["DATA_TYPE"].ToString();
                    string isNullable = reader["IS_NULLABLE"].ToString();
                    string columnKey = reader["COLUMN_KEY"].ToString();
                    Console.WriteLine($"Kolom: {columnName}, Type: {dataType}, Nullable: {isNullable}, Key: {columnKey}");
                }
            }
        }
    }

    public static void GetFunctions()
    {
        string query = $"SELECT ROUTINE_NAME, ROUTINE_TYPE FROM information_schema.routines WHERE routine_schema = '{DBNames.Database}';";

        MySqlConnection connection = new(DBConnect.ConnectionString);
        connection.Open();

        using (var command = new MySqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string routineName = reader["ROUTINE_NAME"].ToString();
                    string routineType = reader["ROUTINE_TYPE"].ToString(); // 'FUNCTION' or 'PROCEDURE'
                    Console.WriteLine($"Routine: {routineName}, Type: {routineType}");
                    //TODO: Dump result in Function, or Procedure list
                }
            }
        }
    }
}
