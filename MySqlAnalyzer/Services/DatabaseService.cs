using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using MySqlAnalyzer.Models;
using MySqlAnalyzer.Repositories;

namespace MySqlAnalyzer.Services
{
    public class DatabaseService
    {
        private readonly DatabaseRepository _repository;

        public DatabaseService(string connectionString)
        {
            _repository = new DatabaseRepository(connectionString);
        }

        public async Task<DatabaseModel> AnalyzeDatabaseAsync(string databaseName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(databaseName))
                    throw new ArgumentException("Database name cannot be empty");

                // Voer de analyse asynchroon uit
                return await Task.Run(() => _repository.GetCompleteDatabase(databaseName));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error analyzing database {databaseName}: {ex.Message}");
                throw new ApplicationException($"Failed to analyze database {databaseName}. See inner exception for details.", ex);
            }
        }

        public async Task<List<string>> GetAvailableDatabasesAsync()
        {
            try
            {
                // Voer de database query asynchroon uit
                return await Task.Run(() => _repository.GetDatabaseNames());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting database list: {ex.Message}");
                throw new ApplicationException("Failed to retrieve database list. See inner exception for details.", ex);
            }
        }
    }
}