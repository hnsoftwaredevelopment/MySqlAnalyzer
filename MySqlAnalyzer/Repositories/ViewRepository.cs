using System;
using System.Collections.Generic;
using System.Data;

using MySqlAnalyzer.Models;

namespace MySqlAnalyzer.Repositories
{
    public class ViewRepository : BaseRepository
    {
        public ViewRepository(string connectionString) : base(connectionString) { }

        public List<ViewModel> GetViews(string databaseName)
        {
            var views = new List<ViewModel>();

            string query = $@"
                SELECT 
                    TABLE_NAME,
                    VIEW_DEFINITION,
                    CHECK_OPTION,
                    IS_UPDATABLE,
                    DEFINER,
                    SECURITY_TYPE,
                    CHARACTER_SET_CLIENT,
                    COLLATION_CONNECTION
                FROM information_schema.VIEWS 
                WHERE TABLE_SCHEMA = '{databaseName}'
                ORDER BY TABLE_NAME";

            var dataTable = ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                var view = new ViewModel
                {
                    Name = GetStringSafe(row, "TABLE_NAME"),
                    Schema = databaseName,
                    Definition = GetStringSafe(row, "VIEW_DEFINITION"),
                    CheckOption = GetStringSafe(row, "CHECK_OPTION"),
                    IsUpdatable = GetBoolSafe(row, "IS_UPDATABLE"),
                    Definer = GetStringSafe(row, "DEFINER"),
                    SecurityType = GetStringSafe(row, "SECURITY_TYPE"),
                    CharacterSet = GetStringSafe(row, "CHARACTER_SET_CLIENT"),
                    Collation = GetStringSafe(row, "COLLATION_CONNECTION")
                };

                views.Add(view);
            }

            return views;
        }

        public List<ColumnModel> GetViewColumns(string databaseName, string viewName)
        {
            // Gebruik dezelfde methode als voor tabellen
            var tableRepo = new TableRepository(_connectionString);
            return tableRepo.GetTableColumns(databaseName, viewName);
        }
    }
}