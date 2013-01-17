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
        private Dictionary<string, Table> _otherTables;
        //private Dictionary<string, int> _tables;

        public  Database(string connectionString)
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
            foreach (Table specifiedTable in dataspec.Tables)
            {
                //var existingTable = _otherTables.SingleOrDefault(x=> x.Key == specifiedTable.Name);
                if (!_otherTables.ContainsKey(specifiedTable.Name)) return false;
                if (!VerifyColumns(specifiedTable, _otherTables[specifiedTable.Name])) return false;
            }
            return true;
        }

        private bool VerifyColumns(Table specifiedTable, Table existingTable)
        {
            //var objId = _tables[table.Name];
            //var existingColumns = GetColums(objId);
            foreach (Column specedColumn in specifiedTable.Columns)
            {
                Column existingColumn = FindExistingColumn(specedColumn.Name, existingTable.Columns);
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

        private Column FindExistingColumn(string name, IEnumerable<Column> existingColumns)
        {
            return existingColumns.FirstOrDefault(existingColumn => existingColumn.Name == name);
        }

        private IList<Column> GetColumns(int objId)
        {
            /* To get primary keys/index
             * 
select unique_index_id from sys.key_constraints where parent_object_id = 741577680 and type = 'PK'
select * from sys.indexes  where object_id = 741577680 and index_id = 1
--741577680
SELECT * FROM sys.index_columns where object_id = 741577680 and index_id = 1

SELECT * FROM sys.columns where object_id = 741577680 and column_id in (1,2)
             * */

            var columns = new List<Column>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"SELECT c.Name, t.Name [type], c.max_length, c.is_nullable, c.is_identity, ic.Seed_Value, ic.increment_value 
                                            FROM Sys.columns c
                                            Inner Join Sys.Types t on c.user_Type_id = t.user_type_id
                                            Left Join [sys].[identity_columns] ic ON ic.object_id = c.object_id and ic.column_id = c.column_id 
                                            where c.object_id = @objectId", conn);
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
                        var column = new Column(reader.GetString(nameOrd));
                        SqlDbType type;
                        var typeStr = reader.GetString(typeOrd);

                        const bool ignoreCase = true;
                        if (Enum.TryParse(typeStr, ignoreCase, out type))
                        {
                            column.Type = type;
                        }
                        if (typeStr[0] == 'n')// unicode
                        {
                            column.Length = reader.GetInt16(lengthOrd)/2;
                        }
                        else
                        {
                            column.Length = reader.GetInt16(lengthOrd);
                        }
                         column.AllowNulls = reader.GetBoolean(allowNullsOrd);

                        //Only Int Identities supported at the moment
                        if (reader.GetBoolean(identityOrd) && column.Type == SqlDbType.Int)
                        {
                            var identity = new IdentityInt(reader.GetInt32(identitySeedOrd),
                                                             reader.GetInt32(identityIncrementOrd));
                            column.Identity = identity;
                        }

                        columns.Add(column);
                    }
                }
            }
            return columns;
        }
        /*
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
                                            where c.object_id = @objectId", conn);
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
                            var identity = new IdentityInt(reader.GetInt32(identitySeedOrd),
                                                             reader.GetInt32(identityIncrementOrd));
                            column.Identity = identity;
                        }

                        columns.Add(column);
                    }
                }
            }
            return columns;
        }
        */
        /*
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
        */
        private void GetSchemaInformation()
        {
            //_tables = new Dictionary<string, int>();
            _otherTables = new Dictionary<string, Table>();
            var tables = new Dictionary<int, Table>();
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
                        var tableName = reader.GetString(nameOrd);
                        var objId = reader.GetInt32(objectIdOrd);
                        //_tables.Add(tableName, objId);
                        tables.Add(objId, new Table(tableName));
                    }
                }
            }
            foreach (var table in tables)
            {
                table.Value.Columns = GetColumns(table.Key);
                _otherTables.Add(table.Value.Name, table.Value);
            }
        }
    }
    
}