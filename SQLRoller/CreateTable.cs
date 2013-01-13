using System;

namespace SQLRoller
{
    public class CreateTable
    {
        private readonly string _tableName;
        private readonly IScopeResolver _scopeResolver;
        private readonly string _schemaName;
        private readonly string _dbName;

        public CreateTable(string tableName, IScopeResolver scopeResolver)
        {
            _tableName = tableName;
            _scopeResolver = scopeResolver;
        }

        public CreateTable(string tableName, string schemaName)
        {
            _tableName = tableName;
            _schemaName = schemaName;
        }

        public CreateTable(string tableName, string schemaName, string dbName)
        {
            _tableName = tableName;
            _schemaName = schemaName;
            _dbName = dbName;
        }

        public string GetReleaseSql()
        {
            return String.Format("CREATE TABLE {0}[{1}]", _scopeResolver.Write(), _tableName);
        }
    }
}
