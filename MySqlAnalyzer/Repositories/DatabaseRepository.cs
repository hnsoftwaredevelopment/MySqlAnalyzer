using MySql.Data.MySqlClient;

using MySqlAnalyzer.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace MySqlAnalyzer.Repositories
{
    public class DatabaseRepository : BaseRepository
    {
        public DatabaseRepository(string connectionString) : base(connectionString) { }

        public DatabaseModel GetCompleteDatabase(string databaseName)
        {
            var database = new DatabaseModel
            {
                Name = databaseName
            };

            // Haal database info op
            var dbInfo = GetDatabaseInfo(databaseName);
            database.DefaultCharacterSet = dbInfo.DefaultCharacterSet;
            database.DefaultCollation = dbInfo.DefaultCollation;

            // Haal alle objecten op
            var tableRepo = new TableRepository(_connectionString);
            var viewRepo = new ViewRepository(_connectionString);
            var procedureRepo = new StoredProcedureRepository(_connectionString);
            var functionRepo = new FunctionRepository(_connectionString);
            var triggerRepo = new TriggerRepository(_connectionString);

            // Tables
            database.Tables = tableRepo.GetTables(databaseName);
            foreach (var table in database.Tables)
            {
                if (table.Name != null)
                {
                    table.Columns = tableRepo.GetTableColumns(databaseName, table.Name);
                    table.Indexes = tableRepo.GetTableIndexes(databaseName, table.Name);
                    table.ForeignKeys = tableRepo.GetTableForeignKeys(databaseName, table.Name);
                }
            }

            // Views
            database.Views = viewRepo.GetViews(databaseName);
            foreach (var view in database.Views)
            {
                if(view.Name != null)
                    view.Columns = viewRepo.GetViewColumns(databaseName, view.Name);
            }

            // Stored Procedures
            database.StoredProcedures = procedureRepo.GetStoredProcedures(databaseName);
            foreach (var procedure in database.StoredProcedures)
            {
                if(procedure.Name != null)
                    procedure.Parameters = procedureRepo.GetProcedureParameters(databaseName, procedure.Name);
            }

            // Functions
            database.Functions = functionRepo.GetFunctions(databaseName);
            foreach (var function in database.Functions)
            {
                if (function.Name != null)
                    function.Parameters = functionRepo.GetFunctionParameters(databaseName, function.Name);
            }

            // Triggers
            database.Triggers = triggerRepo.GetTriggers(databaseName);

            foreach (var table in database.Tables)
            {
                if(table.Name != null) table.Columns = tableRepo.GetTableColumns(databaseName, table.Name);
                if (table.Name != null) table.Indexes = tableRepo.GetTableIndexes(databaseName, table.Name);
                if (table.Name != null) table.ForeignKeys = tableRepo.GetTableForeignKeys(databaseName, table.Name);

                // Genereer CREATE script
                table.GenerateCreateScript();
            }

            return database;
        }

        public List<string> GetDatabaseNames()
        {
            try
            {
                Debug.WriteLine("[DEBUG] DatabaseRepository.GetDatabaseNames() called");
                var databases = new List<string>();
                string query = "SHOW DATABASES";

                Debug.WriteLine("[DEBUG] Executing query...");
                var dataTable = ExecuteQuery(query);
                Debug.WriteLine($"[DEBUG] Query returned {dataTable.Rows.Count} rows");

                foreach (DataRow row in dataTable.Rows)
                {
                    var dbName = GetStringSafe(row, "Database");
                    Debug.WriteLine($"[DEBUG] Raw database name from DB: '{dbName}'");

                    if (string.IsNullOrEmpty(dbName))
                        continue;

                    var lowerDbName = dbName.ToLower();

                    // Filter systeemdatabases
                    if (!lowerDbName.Contains("sys") &&
                        !lowerDbName.Contains("information_schema") &&
                        !lowerDbName.Contains("performance_schema") &&
                        !lowerDbName.Contains("mysql"))
                    {
                        databases.Add(dbName);
                        Debug.WriteLine($"[DEBUG] Added to list: {dbName}");
                    }
                    else
                    {
                        Debug.WriteLine($"[DEBUG] Filtered out (system DB): {dbName}");
                    }
                }

                Debug.WriteLine($"[DEBUG] Returning {databases.Count} databases");
                return databases;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DEBUG] Error in GetDatabaseNames: {ex.Message}");
                Debug.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private DatabaseInfo GetDatabaseInfo(string databaseName)
        {
            var info = new DatabaseInfo();

            string query = $@"
                SELECT 
                    DEFAULT_CHARACTER_SET_NAME,
                    DEFAULT_COLLATION_NAME
                FROM information_schema.SCHEMATA 
                WHERE SCHEMA_NAME = '{databaseName}'";

            var dataTable = ExecuteQuery(query);

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                info.DefaultCharacterSet = GetStringSafe(row, "DEFAULT_CHARACTER_SET_NAME");
                info.DefaultCollation = GetStringSafe(row, "DEFAULT_COLLATION_NAME");
            }

            return info;
        }

        // Helper class voor database info
        private class DatabaseInfo
        {
            public string? DefaultCharacterSet { get; set; }
            public string? DefaultCollation { get; set; }
        }
    }
}