using System;

namespace SQLRoller
{
    public class ScopeResolver : IScopeResolver
    {
        private readonly string _resolvedScope;
        
        public ScopeResolver(string dbName = null, string schemaName = null)
        {
            if (String.IsNullOrEmpty(dbName) && String.IsNullOrEmpty(schemaName))
            {
                _resolvedScope = string.Empty;
            }
            else if (String.IsNullOrEmpty(dbName))
            {
                _resolvedScope = String.Format("[{0}].", schemaName);
            }
            else if(String.IsNullOrEmpty(schemaName))
            {
                _resolvedScope = String.Format("[{0}]..", dbName);
            }
            else
            {
                _resolvedScope = String.Format("[{0}].[{1}].", dbName, schemaName);
            }
        }

        public string Write()
        {
            return _resolvedScope;
        }
    }
}