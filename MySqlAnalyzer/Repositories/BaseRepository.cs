using MySqlCommand = MySql.Data.MySqlClient.MySqlCommand;
using MySqlConnection = MySql.Data.MySqlClient.MySqlConnection;
using MySqlDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;

namespace MySqlAnalyzer.Repositories;

public abstract class BaseRepository
{
    protected readonly string _connectionString;

    protected BaseRepository ( string connectionString )
    {
        _connectionString = connectionString;
    }

    protected MySqlConnection CreateConnection ()
    {
        return new MySqlConnection ( _connectionString );
    }

    protected DataTable ExecuteQuery ( string query, Dictionary<string, object>? parameters = null )
    {
        using ( var connection = new MySqlConnection ( _connectionString ) )
        {
            connection.Open ();
            using ( var command = new MySqlCommand ( query, connection ) )
            {
                if ( parameters != null )
                {
                    foreach ( var param in parameters )
                    {
                        command.Parameters.AddWithValue ( param.Key, param.Value );
                    }
                }

                using ( var adapter = new MySqlDataAdapter ( command ) )
                {
                    var dataTable = new DataTable();
                    adapter.Fill ( dataTable );
                    return dataTable;
                }
            }
        }
    }

    protected async Task<DataTable> ExecuteQueryAsync (
    string query,
    Dictionary<string, object>? parameters = null,
    CancellationToken cancellationToken = default )
    {
        using ( var connection = new MySqlConnection ( _connectionString ) )
        {
            await connection.OpenAsync ( cancellationToken );
            using ( var command = new MySqlCommand ( query, connection ) )
            {
                if ( parameters != null )
                {
                    foreach ( var param in parameters )
                    {
                        command.Parameters.AddWithValue ( param.Key, param.Value );
                    }
                }

                using ( var adapter = new MySqlDataAdapter ( command ) )
                {
                    var dataTable = new DataTable();
                    await Task.Run ( () => adapter.Fill ( dataTable ), cancellationToken );
                    return dataTable;
                }
            }
        }
    }

    protected string? GetStringSafe ( DataRow row, string columnName )
    {
        if ( !row.Table.Columns.Contains ( columnName ) )
            return null;

        return row [ columnName ] == DBNull.Value ? null : Convert.ToString ( row [ columnName ] );
    }

    protected DateTime? GetDateTimeSafe ( DataRow row, string columnName )
    {
        if ( !row.Table.Columns.Contains ( columnName ) )
            return null;

        return row [ columnName ] == DBNull.Value ? ( DateTime? ) null : Convert.ToDateTime ( row [ columnName ] );
    }

    protected long GetLongSafe ( DataRow row, string columnName )
    {
        if ( !row.Table.Columns.Contains ( columnName ) )
            return 0;

        if ( row [ columnName ] == DBNull.Value )
            return 0;

        try
        {
            return Convert.ToInt64 ( row [ columnName ] );
        }
        catch
        {
            return 0;
        }
    }

    protected long? GetNullableLongSafe ( DataRow row, string columnName )
    {
        if ( !row.Table.Columns.Contains ( columnName ) )
            return null;

        return row [ columnName ] == DBNull.Value ? null : Convert.ToInt64 ( row [ columnName ] );
    }

    protected int GetIntSafe ( DataRow row, string columnName )
    {
        if ( !row.Table.Columns.Contains ( columnName ) )
            return 0;

        if ( row [ columnName ] == DBNull.Value )
            return 0;

        try
        {
            return Convert.ToInt32 ( row [ columnName ] );
        }
        catch
        {
            return 0;
        }
    }

    protected int? GetNullableIntSafe ( DataRow row, string columnName )
    {
        if ( !row.Table.Columns.Contains ( columnName ) )
            return null;

        return row [ columnName ] == DBNull.Value ? null : Convert.ToInt32 ( row [ columnName ] );
    }

    protected bool GetBoolSafe ( DataRow row, string columnName )
    {
        if ( !row.Table.Columns.Contains ( columnName ) )
            return false;

        if ( row [ columnName ] == DBNull.Value )
            return false;

        var value = Convert.ToString(row[columnName]);
        if ( string.IsNullOrEmpty ( value ) )
            return false;

        var upperValue = value.ToUpper();
        return upperValue == "YES" || upperValue == "1" || upperValue == "TRUE";
    }

    protected ParameterMode GetParameterMode ( string? mode )
    {
        if ( string.IsNullOrEmpty ( mode ) )
            return ParameterMode.IN; // Default

        return mode.ToUpper () switch
        {
            "IN" => ParameterMode.IN,
            "OUT" => ParameterMode.OUT,
            "INOUT" => ParameterMode.INOUT,
            _ => ParameterMode.IN
        };
    }

    // Helper voor enum parsing met default waarde
    protected T GetEnumSafe<T> ( DataRow row, string columnName, T defaultValue ) where T : struct, Enum
    {
        var stringValue = GetStringSafe(row, columnName);
        if ( string.IsNullOrEmpty ( stringValue ) )
            return defaultValue;

        if ( Enum.TryParse<T> ( stringValue, true, out var result ) )
            return result;

        return defaultValue;
    }

    protected async Task<T> ExecuteInTransactionAsync<T> (
        Func<MySql.Data.MySqlClient.MySqlTransaction, Task<T>> operation,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted )
    {
        using ( var connection = new MySql.Data.MySqlClient.MySqlConnection ( _connectionString ) )
        {
            await connection.OpenAsync ();
            using ( var transaction = await connection.BeginTransactionAsync ( isolationLevel ) )
            {
                try
                {
                    var result = await operation(transaction);
                    await transaction.CommitAsync ();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync ();
                    throw;
                }
            }
        }
    }
}