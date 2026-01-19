using MySqlAnalyzer.Models;

using System;
using System.Collections.Generic;
using System.Data;

namespace MySqlAnalyzer.Repositories
{
    public class TriggerRepository : BaseRepository
    {
        public TriggerRepository(string connectionString) : base(connectionString) { }

        public List<TriggerModel> GetTriggers(string databaseName)
        {
            var triggers = new List<TriggerModel>();

            string query = $@"
                SELECT 
                    TRIGGER_NAME,
                    EVENT_MANIPULATION,
                    EVENT_OBJECT_TABLE,
                    ACTION_TIMING,
                    ACTION_STATEMENT,
                    DEFINER,
                    CHARACTER_SET_CLIENT,
                    COLLATION_CONNECTION,
                    CREATED,
                    SQL_MODE,
                    DEFINER
                FROM information_schema.TRIGGERS 
                WHERE TRIGGER_SCHEMA = '{databaseName}'
                ORDER BY EVENT_OBJECT_TABLE, ACTION_TIMING, EVENT_MANIPULATION";

            var dataTable = ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                var trigger = new TriggerModel
                {
                    Name = GetStringSafe(row, "TRIGGER_NAME"),
                    Schema = databaseName,
                    EventManipulation = GetStringSafe(row, "EVENT_MANIPULATION"),
                    EventObjectTable = GetStringSafe(row, "EVENT_OBJECT_TABLE"),
                    ActionTiming = GetStringSafe(row, "ACTION_TIMING"),
                    ActionStatement = GetStringSafe(row, "ACTION_STATEMENT"),
                    Definer = GetStringSafe(row, "DEFINER"),
                    CharacterSet = GetStringSafe(row, "CHARACTER_SET_CLIENT"),
                    Collation = GetStringSafe(row, "COLLATION_CONNECTION"),
                    Created = GetDateTimeSafe(row, "CREATED") ?? DateTime.MinValue,
                    SqlMode = GetStringSafe(row, "SQL_MODE")
                };

                triggers.Add(trigger);
            }

            return triggers;
        }
    }
}