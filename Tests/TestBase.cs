using Moq;

using MySql.Data.MySqlClient;

using System.Data;

namespace Tests
{
    public abstract class TestBase
    {
        protected Mock<IDbConnection> CreateMockConnection()
        {
            var mockConnection = new Mock<IDbConnection>();
            return mockConnection;
        }

        protected DataTable CreateMockDataTable(params string[] columnNames)
        {
            var dataTable = new DataTable();
            foreach (var columnName in columnNames)
            {
                dataTable.Columns.Add(columnName);
            }
            return dataTable;
        }

        protected void AddRow(DataTable dataTable, params object[] values)
        {
            dataTable.Rows.Add(values);
        }
    }
}