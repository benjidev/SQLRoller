using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SQLRoller
{
    public class Database
    {
        private readonly string _connectionString;
        private Dictionary<string, int> _tables;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Satisfies(DatabaseSpec dataspec)
        {
            GetSchemaInformation();
            return VerifySchema(dataspec);
        }

        private bool VerifySchema(DatabaseSpec dataspec)
        {
            foreach (DatabaseSpec.Table table in dataspec.tables)
            {
                if (!_tables.ContainsKey(table.Name)) return false;
                if (!VerifyColumns(table)) return false;
            }
            return true;
        }

        private bool VerifyColumns(DatabaseSpec.Table table)
        {
            var objId = _tables[table.Name];
            var columns = GetColums(objId);

            return table.Columns.All(columns.Contains);

        }

        private IList<string> GetColums(int objId)
        {
            //SELECT Name FROM Sys.columns where object_id = 309576141
            var columns = new List<string>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT Name FROM Sys.columns where object_id = @objectId", conn);
                cmd.Parameters.Add("@objectId", SqlDbType.Int).Value = objId;

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    var nameOrd = reader.GetOrdinal("Name");
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(nameOrd));
                    }
                }
            }
            return columns;
        }


        private void GetSchemaInformation()
        {
            _tables = new Dictionary<string, int>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("select Name, object_id from sys.tables", conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    var nameOrd = reader.GetOrdinal("Name");
                    var objectIdOrd = reader.GetOrdinal("object_id");
                    while (reader.Read())
                    {
                        _tables.Add(reader.GetString(nameOrd), reader.GetInt32(objectIdOrd));
                    }
                }
            }
        }
    }
}