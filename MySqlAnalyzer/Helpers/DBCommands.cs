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
                    string tableType = reader["TABLE_TYPE"].ToString(); // 'BASE TABLE' voor tabellen, 'VIEW' voor views
                    Console.WriteLine($"Tabel/View: {tableName}, Type: {tableType}");
                }
            }
        }
    }
}
