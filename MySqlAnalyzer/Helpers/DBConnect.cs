namespace MySqlAnalyzer.Helpers;
public class DBConnect
{
    public MySql.Data.MySqlClient.MySqlConnection? connection;

    //public static readonly string server = IpHelper.GetIP();
    public static readonly string server = IpHelper.GetIP();

    public static readonly string database = DBNames.Database;
    public static readonly string port = "3306";
    public static readonly string uid = "root";
    public static readonly string password = "OefenenKHMK24!";

    public static readonly string ConnectionString = $"" +
        $"SERVER = {server}; PORT = {port}; " + $"DATABASE = {database}; UID = {uid}; PASSWORD = {password};";
}