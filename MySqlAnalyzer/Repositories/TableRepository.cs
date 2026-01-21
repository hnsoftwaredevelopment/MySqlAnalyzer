namespace MySqlAnalyzer.Repositories;

public class TableRepository : BaseRepository
{
    public TableRepository ( string connectionString ) : base ( connectionString )
    {
    }

    public List<TableModel> GetTables ( string databaseName )
    {
        string query = @"
            SELECT
                TABLE_NAME,
                TABLE_TYPE,
                ENGINE,
                TABLE_ROWS,
                DATA_LENGTH,
                INDEX_LENGTH,
                CREATE_TIME,
                UPDATE_TIME,
                TABLE_COLLATION,
                TABLE_COMMENT
            FROM information_schema.TABLES
            WHERE TABLE_SCHEMA = @databaseName
            AND TABLE_TYPE = 'BASE TABLE'
            ORDER BY TABLE_NAME";

        // Gebruik parameters voor SQL injection preventie
        var parameters = new Dictionary<string, object>
        {
            { "@databaseName", databaseName }
        };

        var dataTable = ExecuteQuery(query, parameters);
        var tables = new List<TableModel>();

        foreach ( DataRow row in dataTable.Rows )
        {
            var table = new TableModel
            {
                Name = GetStringSafe(row, "TABLE_NAME") ?? string.Empty,
                Schema = databaseName,
                TableType = GetStringSafe(row, "TABLE_TYPE") ?? "BASE TABLE",
                Engine = GetStringSafe(row, "ENGINE") ?? string.Empty,
                TableRows = GetLongSafe(row, "TABLE_ROWS"),
                DataLength = GetLongSafe(row, "DATA_LENGTH"),
                IndexLength = GetLongSafe(row, "INDEX_LENGTH"),
                Created = GetDateTimeSafe(row, "CREATE_TIME") ?? DateTime.MinValue,
                Updated = GetDateTimeSafe(row, "UPDATE_TIME") ?? DateTime.MinValue,
                Collation = GetStringSafe(row, "TABLE_COLLATION"),
                Comment = GetStringSafe(row, "TABLE_COMMENT")
            };

            tables.Add ( table );
        }

        return tables;
    }

    public List<ColumnModel> GetTableColumns ( string databaseName, string tableName )
    {
        var columns = new List<ColumnModel>();

        // Stap 1: Basis column informatie
        string columnsQuery = @"
            SELECT
                COLUMN_NAME,
                ORDINAL_POSITION,
                COLUMN_DEFAULT,
                IS_NULLABLE,
                DATA_TYPE,
                CHARACTER_MAXIMUM_LENGTH,
                NUMERIC_PRECISION,
                NUMERIC_SCALE,
                DATETIME_PRECISION,
                CHARACTER_SET_NAME,
                COLLATION_NAME,
                COLUMN_TYPE,
                COLUMN_KEY,
                EXTRA,
                COLUMN_COMMENT,
                GENERATION_EXPRESSION
            FROM information_schema.COLUMNS
            WHERE TABLE_SCHEMA = @databaseName
            AND TABLE_NAME = @tableName
            ORDER BY ORDINAL_POSITION";

        var columnsParams = new Dictionary<string, object>
        {
            { "@databaseName", databaseName },
            { "@tableName", tableName }
        };

        var columnsDataTable = ExecuteQuery(columnsQuery, columnsParams);

        foreach ( DataRow row in columnsDataTable.Rows )
        {
            var column = new ColumnModel
            {
                Name = GetStringSafe(row, "COLUMN_NAME") ?? string.Empty,
                TableName = tableName,
                Schema = databaseName,
                OrdinalPosition = GetIntSafe(row, "ORDINAL_POSITION"),
                DefaultValue = GetStringSafe(row, "COLUMN_DEFAULT"),
                IsNullable = GetBoolSafe(row, "IS_NULLABLE"),
                DataType = GetStringSafe(row, "DATA_TYPE") ?? string.Empty,
                CharacterMaximumLength = GetLongSafe(row, "CHARACTER_MAXIMUM_LENGTH"),
                NumericPrecision = GetIntSafe(row, "NUMERIC_PRECISION"),
                NumericScale = GetIntSafe(row, "NUMERIC_SCALE"),
                DateTimePrecision = GetIntSafe(row, "DATETIME_PRECISION"),
                CharacterSet = GetStringSafe(row, "CHARACTER_SET_NAME"),
                Collation = GetStringSafe(row, "COLLATION_NAME"),
                ColumnType = GetStringSafe(row, "COLUMN_TYPE") ?? string.Empty,
                ColumnKey = GetStringSafe(row, "COLUMN_KEY"),
                Extra = GetStringSafe(row, "EXTRA"),
                Comment = GetStringSafe(row, "COLUMN_COMMENT"),
                GenerationExpression = GetStringSafe(row, "GENERATION_EXPRESSION")
            };

            columns.Add ( column );
        }
        var foreignKeys = GetTableForeignKeys(databaseName, tableName);
        AddForeignKeyInfoToColumns ( columns, foreignKeys );

        return columns;
    }

    public List<IndexModel> GetTableIndexes ( string databaseName, string tableName )
    {
        // Je bestaande implementatie is goed
        // Alleen parameterized query maken:
        string query = @"
            SELECT
                INDEX_NAME,
                NON_UNIQUE,
                INDEX_TYPE,
                COLUMN_NAME,
                SEQ_IN_INDEX,
                COLLATION,
                CARDINALITY,
                SUB_PART,
                NULLABLE,
                INDEX_COMMENT
            FROM information_schema.STATISTICS
            WHERE TABLE_SCHEMA = @databaseName
            AND TABLE_NAME = @tableName
            ORDER BY INDEX_NAME, SEQ_IN_INDEX";

        var parameters = new Dictionary<string, object>
        {
            { "@databaseName", databaseName },
            { "@tableName", tableName }
        };

        var dataTable = ExecuteQuery(query, parameters);

        // ... rest van je bestaande code
        var indexes = new Dictionary<string, IndexModel>();

        foreach ( DataRow row in dataTable.Rows )
        {
            var indexName = GetStringSafe(row, "INDEX_NAME");
            if ( string.IsNullOrEmpty ( indexName ) ) continue;

            if ( !indexes.ContainsKey ( indexName ) )
            {
                var index = new IndexModel
                {
                    Name = indexName,
                    TableName = tableName,
                    Schema = databaseName,
                    IsUnique = !GetBoolSafe(row, "NON_UNIQUE"),
                    IndexType = GetStringSafe(row, "INDEX_TYPE") ?? string.Empty,
                    Cardinality = GetLongSafe(row, "CARDINALITY"),
                    Comment = GetStringSafe(row, "INDEX_COMMENT"),
                    Columns = new List<IndexColumnModel>()
                };

                indexes [ indexName ] = index;
            }

            var columnName = GetStringSafe(row, "COLUMN_NAME");
            if ( !string.IsNullOrEmpty ( columnName ) )
            {
                var indexColumn = new IndexColumnModel
                {
                    ColumnName = columnName,
                    OrdinalPosition = GetIntSafe(row, "SEQ_IN_INDEX"),
                    IsDescending = GetStringSafe(row, "COLLATION") == "D",
                    SubPart = GetIntSafe(row, "SUB_PART")
                };

                indexes [ indexName ].Columns.Add ( indexColumn );
            }
        }

        // Mark primary key
        foreach ( var index in indexes.Values )
        {
            index.IsPrimary = index.Name == "PRIMARY";
        }

        return new List<IndexModel> ( indexes.Values );
    }

    public List<ForeignKeyModel> GetTableForeignKeys ( string databaseName, string tableName )
    {
        var foreignKeys = new Dictionary<string, ForeignKeyModel>();

        // Query 1: Haal foreign key columns op
        string query = @"
            SELECT
                kcu.CONSTRAINT_NAME,
                kcu.COLUMN_NAME,
                kcu.REFERENCED_TABLE_SCHEMA,
                kcu.REFERENCED_TABLE_NAME,
                kcu.REFERENCED_COLUMN_NAME
            FROM information_schema.KEY_COLUMN_USAGE kcu
            WHERE kcu.TABLE_SCHEMA = @databaseName
            AND kcu.TABLE_NAME = @tableName
            AND kcu.REFERENCED_TABLE_NAME IS NOT NULL
            ORDER BY kcu.CONSTRAINT_NAME, kcu.ORDINAL_POSITION";

        var parameters = new Dictionary<string, object>
        {
            { "@databaseName", databaseName },
            { "@tableName", tableName }
        };

        var dataTable = ExecuteQuery(query, parameters);

        foreach ( DataRow row in dataTable.Rows )
        {
            var constraintName = GetStringSafe(row, "CONSTRAINT_NAME");
            if ( string.IsNullOrEmpty ( constraintName ) ) continue;

            if ( !foreignKeys.ContainsKey ( constraintName ) )
            {
                var foreignKey = new ForeignKeyModel
                {
                    Name = constraintName,
                    TableName = tableName,
                    Schema = databaseName,
                    ReferencedTable = GetStringSafe(row, "REFERENCED_TABLE_NAME") ?? string.Empty,
                    ReferencedSchema = GetStringSafe(row, "REFERENCED_TABLE_SCHEMA") ?? string.Empty,
                    ColumnMappings = []
                };

                foreignKeys [ constraintName ] = foreignKey;
            }

            var columnName = GetStringSafe(row, "COLUMN_NAME");
            var referencedColumnName = GetStringSafe(row, "REFERENCED_COLUMN_NAME");

            if ( !string.IsNullOrEmpty ( columnName ) && !string.IsNullOrEmpty ( referencedColumnName ) )
            {
                var mapping = new ForeignKeyColumnMappingModel
                {
                    ColumnName = columnName,
                    ReferencedColumnName = referencedColumnName
                };

                foreignKeys [ constraintName ].ColumnMappings.Add ( mapping );
            }
        }

        if ( foreignKeys.Count > 0 )
        {
            var constraintNames = string.Join(",", foreignKeys.Keys.Select(k => $"'{k}'"));

            string rulesQuery = $@"
                SELECT
                    CONSTRAINT_NAME,
                    UPDATE_RULE,
                    DELETE_RULE
                FROM information_schema.REFERENTIAL_CONSTRAINTS
                WHERE CONSTRAINT_SCHEMA = '{databaseName}'
                AND CONSTRAINT_NAME IN ({constraintNames})";

            var rulesDataTable = ExecuteQuery(rulesQuery);

            foreach ( DataRow row in rulesDataTable.Rows )
            {
                var constraintName = GetStringSafe(row, "CONSTRAINT_NAME");
                if ( !string.IsNullOrEmpty ( constraintName ) && foreignKeys.ContainsKey ( constraintName ) )
                {
                    foreignKeys [ constraintName ].UpdateRule = GetStringSafe ( row, "UPDATE_RULE" ) ?? "NO ACTION";
                    foreignKeys [ constraintName ].DeleteRule = GetStringSafe ( row, "DELETE_RULE" ) ?? "NO ACTION";
                }
            }
        }

        return [ .. foreignKeys.Values ];
    }

    private void AddForeignKeyInfoToColumns (
        List<ColumnModel> columns,
        List<ForeignKeyModel> foreignKeys )
    {
        foreach ( var fk in foreignKeys )
        {
            foreach ( var mapping in fk.ColumnMappings )
            {
                var column = columns.FirstOrDefault(c =>
                    c.Name == mapping.ColumnName &&
                    c.TableName == fk.TableName);

                if ( column != null )
                {
                    // Markeer als foreign key
                    column.IsForeignKey = true;
                    column.ForeignKeyName = fk.Name;
                    column.ReferencedTable = fk.ReferencedTable;
                    column.ReferencedColumn = mapping.ReferencedColumnName;
                    column.UpdateRule = fk.UpdateRule;
                    column.DeleteRule = fk.DeleteRule;
                }
            }
        }
    }

    public string GetColumnKeyType ( ColumnModel column )
    {
        if ( column.ColumnKey?.Contains ( "PRI" ) == true )
            return "Primary Key";

        if ( column.IsForeignKey )
            return $"Foreign Key → {column.ReferencedTable}.{column.ReferencedColumn}";

        if ( column.ColumnKey?.Contains ( "UNI" ) == true )
            return "Unique";

        if ( column.ColumnKey?.Contains ( "MUL" ) == true )
            return "Indexed";

        return string.Empty;
    }
}