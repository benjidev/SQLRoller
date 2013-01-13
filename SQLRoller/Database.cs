using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SQLRoller.Specify;

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
            foreach (Table table in dataspec.Tables)
            {
                if (!_tables.ContainsKey(table.Name)) return false;
                if (!VerifyColumns(table)) return false;
            }
            return true;
        }

        private bool VerifyColumns(Table table)
        {
            var objId = _tables[table.Name];
            var existingColumns = GetColums(objId);
            foreach (Column specedColumn in table.Columns)
            {
                SchemaColumn existingColumn = FindExistingColumn(specedColumn.Name, existingColumns);
                if (existingColumn == null)
                {
                    return false;
                }

                if (!specedColumn.IsSatisfiedBy(existingColumn))
                {
                    return false;
                }
            }
            return true;
        }

        private SchemaColumn FindExistingColumn(string name, IEnumerable<SchemaColumn> existingColumns)
        {
            return existingColumns.FirstOrDefault(existingColumn => existingColumn.Name == name);
        }


        private IList<SchemaColumn> GetColums(int objId)
        {
            //SELECT Name FROM Sys.columns where object_id = 309576141
            var columns = new List<SchemaColumn>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"SELECT c.Name, t.Name [type], c.max_length, c.is_nullable, c.is_identity, ic.Seed_Value, ic.increment_value 
                                            FROM Sys.columns c
                                            Inner Join Sys.Types t on c.user_Type_id = t.user_type_id
                                            Left Join [sys].[identity_columns] ic ON ic.object_id = c.object_id and ic.column_id = c.column_id 
                                            where object_id = @objectId", conn);
                cmd.Parameters.Add("@objectId", SqlDbType.Int).Value = objId;

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    var nameOrd = reader.GetOrdinal("Name");
                    var typeOrd = reader.GetOrdinal("type");
                    var lengthOrd = reader.GetOrdinal("max_length");
                    var allowNullsOrd = reader.GetOrdinal("is_nullable");
                    var identityOrd = reader.GetOrdinal("is_identity");
                    var identitySeedOrd = reader.GetOrdinal("Seed_Value");
                    var identityIncrementOrd = reader.GetOrdinal("increment_value");
                    while (reader.Read())
                    {
                        var column = new SchemaColumn(reader.GetString(nameOrd),
                            reader.GetString(typeOrd),
                            reader.GetInt16(lengthOrd),
                            reader.GetBoolean(allowNullsOrd));

                        //Only Int Identities supported at the moment
                        if (reader.GetBoolean(identityOrd) && column.Type.Equals("int", StringComparison.OrdinalIgnoreCase))
                        {
                            var identity = new Identity<int>(reader.GetInt32(identitySeedOrd),
                                                             reader.GetInt32(identityIncrementOrd));
                            column.Identity = identity;
                        }

                        columns.Add(column);
                    }
                }
            }
            return columns;
        }

        public class SchemaColumn
        {
            private int _length;

            public SchemaColumn(string name, string type, int length, bool allowNulls)
            {
                Name = name;
                Type = type;
                Length = length;
                AllowNulls = allowNulls;
            }
            public string Name { get; private set; }
            public string Type { get; private set; }
            public bool AllowNulls { get; private set; }
            public int Length
            {
                get
                {
                    if (Type[0] == 'n')
                    {
                        //for nvarchar and nchar the unicode representation takes 2 bytes per character
                        return _length/2;
                    }
                    return _length;
                }
                private set { _length = value; }
            }

            public Identity Identity { get; set; }
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