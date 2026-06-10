namespace MySqlAnalyzer.Repositories;

public class ViewRepository : BaseRepository
{
    public ViewRepository ( string connectionString ) : base ( connectionString )
    {
    }

    /// <summary>
    /// Gets all views from the specified database
    /// </summary>
    public async Task<List<ViewModel>> GetViewsAsync ( string databaseName )
    {
        var views = new List<ViewModel>();

        const string query = @"
                SELECT 
                    TABLE_NAME,
                    IS_UPDATABLE,
                    VIEW_DEFINITION,
                    CHECK_OPTION,
                    SECURITY_TYPE,
                    DEFINER,
                    CHARACTER_SET_CLIENT,
                    COLLATION_CONNECTION,
                    CREATED,
                    LAST_ALTERED
                FROM INFORMATION_SCHEMA.VIEWS 
                WHERE TABLE_SCHEMA = @DatabaseName
                ORDER BY TABLE_NAME";

        var parameters = new Dictionary<string, object>
        {
            { "@databaseName", databaseName }
        };

        var dataTable = await ExecuteQueryAsync(query, parameters);

        if ( dataTable == null || dataTable.Rows.Count == 0 )
        {
            return views;
        }

        foreach ( DataRow row in dataTable.Rows )
        {
            var view = new ViewModel
            {
                ViewName = GetStringSafe(row, "TABLE_NAME"),
                IsUpdatable = GetStringSafe(row, "IS_UPDATABLE") == "YES",
                Definition = GetStringSafe(row, "VIEW_DEFINITION"),
                CheckOption = GetStringSafe(row, "CHECK_OPTION"),
                SecurityType = GetStringSafe(row, "SECURITY_TYPE"),
                Definer = GetStringSafe(row, "DEFINER"),
                CharacterSet = GetStringSafe(row, "CHARACTER_SET_CLIENT"),
                Collation = GetStringSafe(row, "COLLATION_CONNECTION")
            };

            // Handle nullable dates
            if ( !row.IsNull ( "CREATED" ) )
            {
                view.Created = Convert.ToDateTime ( row [ "CREATED" ] );
            }

            if ( !row.IsNull ( "LAST_ALTERED" ) )
            {
                view.LastAltered = Convert.ToDateTime ( row [ "LAST_ALTERED" ] );
            }

            views.Add ( view );
        }

        return views;
    }

    /// <summary>
    /// Gets detailed information about a specific view
    /// </summary>
    public async Task<ViewModel?> GetViewDetailsAsync ( string databaseName, string viewName )
    {
        const string query = @"
                SELECT 
                    TABLE_NAME,
                    IS_UPDATABLE,
                    VIEW_DEFINITION,
                    CHECK_OPTION,
                    SECURITY_TYPE,
                    DEFINER,
                    CHARACTER_SET_CLIENT,
                    COLLATION_CONNECTION,
                    CREATED,
                    LAST_ALTERED
                FROM INFORMATION_SCHEMA.VIEWS 
                WHERE TABLE_SCHEMA = @DatabaseName 
                    AND TABLE_NAME = @ViewName";

        var parameters = new Dictionary<string, object>
        {
            { "@databaseName", databaseName },
            { "@viewName", viewName }
        };

        var dataTable = await ExecuteQueryAsync(query, parameters);

        if ( dataTable == null || dataTable.Rows.Count == 0 )
        {
            return null;
        }

        var row = dataTable.Rows[0];
        var view = new ViewModel
        {
            ViewName = GetStringSafe(row, "TABLE_NAME"),
            IsUpdatable = GetStringSafe(row, "IS_UPDATABLE") == "YES",
            Definition = GetStringSafe(row, "VIEW_DEFINITION"),
            CheckOption = GetStringSafe(row, "CHECK_OPTION"),
            SecurityType = GetStringSafe(row, "SECURITY_TYPE"),
            Definer = GetStringSafe(row, "DEFINER"),
            CharacterSet = GetStringSafe(row, "CHARACTER_SET_CLIENT"),
            Collation = GetStringSafe(row, "COLLATION_CONNECTION")
        };

        // Handle nullable dates
        if ( !row.IsNull ( "CREATED" ) )
        {
            view.Created = Convert.ToDateTime ( row [ "CREATED" ] );
        }

        if ( !row.IsNull ( "LAST_ALTERED" ) )
        {
            view.LastAltered = Convert.ToDateTime ( row [ "LAST_ALTERED" ] );
        }

        return view;
    }

    /// <summary>
    /// Gets all columns for a specific view
    /// </summary>
    public async Task<List<ViewColumn>> GetViewColumnsAsync ( string databaseName, string viewName )
    {
        var columns = new List<ViewColumn>();

        const string query = @"
                SELECT 
                    COLUMN_NAME,
                    DATA_TYPE,
                    IS_NULLABLE,
                    COLUMN_DEFAULT,
                    COLUMN_TYPE,
                    EXTRA,
                    COLUMN_COMMENT,
                    ORDINAL_POSITION,
                    CHARACTER_SET_NAME,
                    COLLATION_NAME
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_SCHEMA = @DatabaseName 
                    AND TABLE_NAME = @ViewName
                ORDER BY ORDINAL_POSITION";

        var parameters = new Dictionary<string, object>
        {
            { "@databaseName", databaseName },
            { "@viewName", viewName }
        };

        var dataTable = await ExecuteQueryAsync(query, parameters);

        if ( dataTable == null || dataTable.Rows.Count == 0 )
        {
            return columns;
        }

        foreach ( DataRow row in dataTable.Rows )
        {
            var column = new ViewColumn
            {
                ColumnName = GetStringSafe(row, "COLUMN_NAME"),
                DataType = GetStringSafe(row, "DATA_TYPE"),
                IsNullable = GetStringSafe(row, "IS_NULLABLE") == "YES",
                ColumnType = GetStringSafe(row, "COLUMN_TYPE"),
                OrdinalPosition = Convert.ToInt32(row["ORDINAL_POSITION"])
            };

            // Handle nullable string columns
            if ( !row.IsNull ( "COLUMN_DEFAULT" ) )
            {
                column.DefaultValue = row [ "COLUMN_DEFAULT" ]?.ToString ();
            }

            if ( !row.IsNull ( "EXTRA" ) )
            {
                column.Extra = row [ "EXTRA" ]?.ToString ();
            }

            if ( !row.IsNull ( "COLUMN_COMMENT" ) )
            {
                column.Comment = row [ "COLUMN_COMMENT" ]?.ToString ();
            }

            columns.Add ( column );
        }

        return columns;
    }

    /// <summary>
    /// Gets the SQL definition of a view
    /// </summary>
    public async Task<string> GetViewDefinitionAsync ( string databaseName, string viewName )
    {
        const string query = @"
                SELECT VIEW_DEFINITION
                FROM INFORMATION_SCHEMA.VIEWS 
                WHERE TABLE_SCHEMA = @DatabaseName 
                    AND TABLE_NAME = @ViewName";

        var parameters = new Dictionary<string, object>
        {
            { "@databaseName", databaseName },
            { "@viewName", viewName }
        };

        var dataTable = await ExecuteQueryAsync(query, parameters);

        if ( dataTable == null || dataTable.Rows.Count == 0 )
        {
            return string.Empty;
        }

        return GetStringSafe ( dataTable.Rows [ 0 ], "VIEW_DEFINITION" );
    }

    /// <summary>
    /// Gets the formatted SQL definition of a view
    /// </summary>
    public async Task<string> GetFormattedViewDefinitionAsync ( string databaseName, string viewName )
    {
        var definition = await GetViewDefinitionAsync(databaseName, viewName);

        if ( string.IsNullOrEmpty ( definition ) )
        {
            return string.Empty;
        }

        return FormatSqlDefinition ( definition );
    }

    /// <summary>
    /// Formats SQL definition for better readability
    /// </summary>
    private static string FormatSqlDefinition ( string sql )
    {
        if ( string.IsNullOrWhiteSpace ( sql ) )
        {
            return string.Empty;
        }

        // Simple formatting
        var formatted = sql
            .Replace("SELECT ", "\nSELECT ")
            .Replace(" FROM ", "\nFROM ")
            .Replace(" WHERE ", "\nWHERE ")
            .Replace(" JOIN ", "\nJOIN ")
            .Replace(" GROUP BY ", "\nGROUP BY ")
            .Replace(" ORDER BY ", "\nORDER BY ")
            .Replace(" UNION ", "\nUNION\n");

        return formatted.Trim ();
    }

    /// <summary>
    /// Safely gets a string value from a DataRow
    /// </summary>
    private static string GetStringSafe ( DataRow row, string columnName )
    {
        if ( row == null || row.Table.Columns.Contains ( columnName ) == false || row.IsNull ( columnName ) )
        {
            return string.Empty;
        }

        return row [ columnName ]?.ToString () ?? string.Empty;
    }
}